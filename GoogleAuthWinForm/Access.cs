using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Windows.Forms;

namespace GoogleAuthWinForm
{
    public partial class Access : Form
    {
        public string email,nome;

        public Access()
        {
            InitializeComponent();

            AccessToken.Text = "";
            refreshToken.Text = "";
            Expire.Text = "";
        }

        private void btnAuth_Click(object sender, EventArgs e)
        {
            Auth m = new Auth();
            var result = m.ShowDialog();

            if (result == DialogResult.OK)
            {
                AccessToken.Text = m.access.Access_token;
                refreshToken.Text = m.access.refresh_token;

                if (DateTime.Now < m.access.created.AddHours(1))
                {
                    Expire.Text = m.access.created.AddHours(1).Subtract(DateTime.Now).Minutes.ToString();
                    getgoogleplususerdataSer(AccessToken.Text);
                }
            }


        }

        private void Access_Load(object sender, EventArgs e)
        {

        }

        private async void getgoogleplususerdataSer(string access_token)
        {
            try
            {
                HttpClient client = new HttpClient();
                var urlProfile = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + access_token;

                client.CancelPendingRequests();
                HttpResponseMessage output = await client.GetAsync(urlProfile);

                if (output.IsSuccessStatusCode)
                {
                    string outputData = await output.Content.ReadAsStringAsync();
                    GoogleUserOutputData serStatus = JsonConvert.DeserializeObject<GoogleUserOutputData>(outputData);

                    if (serStatus != null)
                    {
                        nome = serStatus.name;
                        email = serStatus.email;
                        this.label5.Text = email;
                        this.label7.Text = nome;
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                //catching the exception
            }
        }

        public class GoogleUserOutputData
        {
            public string id { get; set; }
            public string name { get; set; }
            public string given_name { get; set; }
            public string email { get; set; }
            public string picture { get; set; }
        }

    }
}
