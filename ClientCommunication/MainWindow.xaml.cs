using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using ServerApi.Models;
using System.IO;
using System.Net;
using ClientCommunication.Models;
using System.Security.Policy;
using static System.Net.Mime.MediaTypeNames;

namespace ClientCommunication
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly HttpClient clientconnect;
        HttpResponseMessage response;
        UserDbModel loggeduser;
        List<DataDbModel> userchats = new List<DataDbModel>();
        List<string> wiadomoscilista = new List<string>();
        List<TextBox> wiadomoscibox = new List<TextBox>();
        int openchatid = 0;

        string ip = "localhost"; //192.168.123.42
        string port = "7116";

        int flagForChat = 0;


        bool testmode = true;

        List<UserForUser> userstochat = new List<UserForUser>();

        public MainWindow()
        {
            InitializeComponent();

            if (testmode)
            {
                LoginBox.Text = "string";
                HasloBox.Text = "string";
            }

            ListClient.Visibility = Visibility.Hidden;
            
            zalogowano.Visibility = Visibility.Hidden;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }; // Uwaga: używaj tylko w środowisku deweloperskim
            clientconnect = new HttpClient(handler);



        }

        private async void Choose_Click(object sender, RoutedEventArgs e)
        {
            if (sendtextbox.Text != "")
            {
                string texttosend=sendtextbox.Text;

                ChatDbModel model = new ChatDbModel();

                model.chatid = openchatid;
                model.message = texttosend;
                model.sender = loggeduser.name;

                var json = JsonSerializer.Serialize(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");



                // Wysłanie żądania POST
                try
                {

                    var response = await clientconnect.PostAsync($"https://{ip}:{port}/api/Chats", content);


                    response.EnsureSuccessStatusCode();

                    //string responseBody = await response.Content.ReadAsStringAsync(); <- ten kod wywola blad i nie zadziala kod post
                    await refreshconverstaion();
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Błąd podczas wysyłania wiadomości: {ex.Message}");
                }

            }
        }

        private async Task refreshconverstaion()
        {
            var textBoxes = ChatCanvas.Children.OfType<TextBox>().ToList();

            // Usuwamy każdy TextBox z Canvas
            foreach (var textBox in textBoxes)
            {
                ChatCanvas.Children.Remove(textBox);
            }
            if (ListClient.SelectedItem != null)
            {
                string nameofelement = ListClient.SelectedItem.ToString();
                chatbox.Visibility = Visibility.Visible;
                Wyslijbtn.Visibility = Visibility.Visible;
                sendtextbox.Visibility = Visibility.Visible;
                try
                {
                    int idchat = userchats[ListClient.SelectedIndex].iddata;
                    //https://localhost:7116/api/Chats/

                    response = await clientconnect.GetAsync($"https://{ip}:{port}/api/Chats/{idchat}");
                    response.EnsureSuccessStatusCode();


                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var wiadomosci = JsonSerializer.Deserialize<List<ChatDbModel>>(jsonResponse);



                    double ile = 0;
                    int ilosc = 0;
                    int czy1 = 0;
                    // Dodawanie danych do ListBox
                    foreach (var item in wiadomosci)
                    {
                        chatbox1.Visibility = Visibility.Visible;

                        openchatid = item.chatid;
                        /*Point topLeft = chatbox1.TranslatePoint(new Point(0, 0), this);
                        double left = topLeft.X; 
                        double top = topLeft.Y; 

                        Point bottomRight = chatbox1.TranslatePoint(new Point(chatbox1.ActualWidth, chatbox1.ActualHeight), this);
                        double right = bottomRight.X; 
                        double bottom = bottomRight.Y;*/


                        //MessageBox.Show($"Lewa: {left}, Górna: {top}, Prawa: {right}, Dolna: {bottom}");
                        if (item.sender != loggeduser.name)
                        {
                            TextBox textBox = new TextBox
                            {
                                MaxWidth = 300,
                                Width = 200,
                                FontSize=24,
                                TextWrapping = TextWrapping.Wrap,
                                IsReadOnly = true,
                                Text = item.message

                            };
                            ilosc++;
                            czy1 = 1;
                            Canvas.SetLeft(textBox, 10);
                            Canvas.SetTop(textBox, ile);
                            ChatCanvas.Children.Add(textBox);
                            ile = ile + textBox.ActualHeight + 20;
                        }
                        else
                        {
                            TextBox textBox = new TextBox
                            {
                                MaxWidth = 300,
                                Width = 200,
                                FontSize = 24,
                                TextWrapping = TextWrapping.Wrap,
                                IsReadOnly=true,
                                Text = item.message,
                            };
                            Canvas.SetLeft(textBox, this.ActualWidth - (this.ActualWidth / 2));
                            Canvas.SetTop(textBox, ile);

                            ile = ile + textBox.ActualHeight *2+ 60;
                            ChatCanvas.Children.Add(textBox);

                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Prawdopodbnie blad serwera \n error: " + ex.ToString());
                }

            }

            ListClient.Items.Clear();
            foreach(var currentitem in userchats)
            {
                ListClient.Items.Add(currentitem.title);
            }
        }

        async Task setchats()
        {
            try
            {

                /*response = await clientconnect.GetAsync($"https://{ip}:{port}/api/datas/userdataget?iduser={loggeduser.idduser}");

                response.EnsureSuccessStatusCode();




                var jsonResponse = await response.Content.ReadAsStringAsync();
                userchats = JsonSerializer.Deserialize<List<DataDbModel>>(jsonResponse);*/

                ListClient.Margin = new Thickness(0, 0, 0, 64);
                ListClient.Items.Clear();
                var response2 = await clientconnect.GetAsync($"https://{ip}:{port}/api/ChatForWhoes/{loggeduser.idduser}");
                response2.EnsureSuccessStatusCode();

                var jsonResponse = await response2.Content.ReadAsStringAsync();
                var chatsforuser = JsonSerializer.Deserialize<List<ChatForWho>>(jsonResponse);

                /*var jsonResponse2 = await response2.Content.ReadAsStringAsync();
                var userchats2 = JsonSerializer.Deserialize<List<ChatForWho>>(jsonResponse);*/
                userchats.Clear();
                // Dodawanie danych do ListBox
                if (chatsforuser != null)
                {
                    int idofchat;
                    foreach (var item in chatsforuser)
                    {
                        idofchat = item.idchat;
                        response = await clientconnect.GetAsync($"https://{ip}:{port}/api/datas/id?id={item.idchat}");
                        response.EnsureSuccessStatusCode();
                        string jsonResponse2 = await response.Content.ReadAsStringAsync();
                        var userData = JsonSerializer.Deserialize<DataDbModel>(jsonResponse2);
                        userchats.Add(userData);
                        ListClient.Items.Add(userData.title);

                        //ListClient.Items.Add(item.title);
                    }
                    //ListClient.ItemsSource = dataList; 
                }


            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                using (StreamWriter sw = new StreamWriter("errors.txt"))
                {
                    sw.WriteLine(DateTime.Now.ToString());
                    sw.WriteLine(e.ToString());
                }
            }

        }
        public async void startUserConv()
        {



        }

        private async void ListClient_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (flagForChat == 2)
            {
                MessageBoxResult result = MessageBox.Show(
                "Czy chcesz zacząc chat z tym uzytkownikiem?",
                "Potwierdz",
                MessageBoxButton.YesNo);

                // Sprawdzenie, co wybrał użytkownik
                if (result == MessageBoxResult.Yes)
                {
                    DataUserDbModel newchat = new DataUserDbModel()
                    {
                        createuser = loggeduser.name,
                        title = $"Chat {loggeduser.name} z {userstochat[ListClient.SelectedIndex].name}",
                        date = DateTime.Now.ToString()
                    };
                    var json = JsonSerializer.Serialize(newchat);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    try
                    {
                        var response = await clientconnect.PostAsync($"https://{ip}:{port}/api/datas", content);

                        response.EnsureSuccessStatusCode();

                        string responseBody = await response.Content.ReadAsStringAsync();
                        if (!(response.StatusCode == HttpStatusCode.OK) || !(response.StatusCode == HttpStatusCode.Created))
                        {
                            MessageBox.Show("Error: " + response);
                        }
                        else
                        {
                            var respone = JsonSerializer.Deserialize<DataDbModel>(responseBody);
                            //https://{ip}:{port}/api/ChatForWhoes

                            UserChatForWho forwhoYOU = new UserChatForWho()
                            {
                                idchat= respone.iddata,
                                forwho=loggeduser.idduser
                            };
                            UserChatForWho forwhoFRIEND = new UserChatForWho()
                            {
                                idchat = respone.iddata,
                                forwho = userstochat[ListClient.SelectedIndex].idduser
                            };
                            string error="";
                            try
                            {

                                var json22 = JsonSerializer.Serialize(forwhoYOU);
                                var content22 = new StringContent(json22, Encoding.UTF8, "application/json");
                                var response22 = await clientconnect.PostAsync($"https://{ip}:{port}/api/ChatForWhoes", content22);
                                error = "error in forwhoYOU";
                                var json23 = JsonSerializer.Serialize(forwhoFRIEND);
                                var content23 = new StringContent(json23, Encoding.UTF8, "application/json");
                                var response23 = await clientconnect.PostAsync($"https://{ip}:{port}/api/ChatForWhoes", content23);
                                error = "error in forwhoFriend";
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Critical error \n please contact IT departament");

                                if (File.Exists("errors.txt"))
                                {
                                    File.AppendAllText("errors.txt", Environment.NewLine+error+Environment.NewLine + ex.Message);
                                }
                                else
                                {
                                    // Inaczej, utwórz plik i zapisz tekst
                                    File.WriteAllText("errors.txt", error + Environment.NewLine + ex.Message);
                                }


                            }



                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        MessageBox.Show($"Błąd podczas tworzenia chatu: {ex.Message}");
                    }
                }
            }
            else
            {
            //MessageBox.Show("Otwieramytenchat");
                //string a = $"https://{ip}:{port}/api/Chats/{userchats[ListClient.SelectedIndex].iddata}";
                await refreshconverstaion();
            }
        }

        private async void Logowanie_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await clientconnect.GetAsync($"https://{ip}:{port}/api/users/name?login={LoginBox.Text}&password={HasloBox.Text}");

                flagForChat = 1;

                try
                {

                    response.EnsureSuccessStatusCode();

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var UserInJson = await response.Content.ReadAsStringAsync();
                        loggeduser = JsonSerializer.Deserialize<UserDbModel>(UserInJson);
                        hideloginpanel();
                        showmainpanel();
                        zalogowano.Content = "Uzytkownik: " + loggeduser.name;

                        setchats();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Bledna nazwa uzytkownika lub haslo \n error: " + ex.ToString());
                }
            }
            catch (Exception ex)
            {
                if (!File.Exists("errors.txt"))
                {

                    StreamWriter sw = new StreamWriter("errors.txt");
                    foreach(var error in ex.Message)
                    {

                        sw.Write(error.ToString());
                    }

                }
                MessageBox.Show("Prawdopodbnie blad serwera \n error: " + ex.ToString());
            }
        }

        async void hideloginpanel()
        {
            this.WindowState = WindowState.Maximized;

            Logowanie.Visibility = Visibility.Hidden;
            LoginBox.Visibility = Visibility.Hidden;
            HasloBox.Visibility = Visibility.Hidden;
            L2Haslo.Visibility = Visibility.Hidden;
            l1Nazwa.Visibility = Visibility.Hidden;

        }
        async void showmainpanel()
        {

            ListClient.Visibility = Visibility.Visible;
            MenuOpen.Visibility = Visibility.Visible;
            zalogowano.Visibility = Visibility.Visible;

        }

        private void chatbox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            sendtextbox.Margin= new Thickness(140, this.ActualHeight-(sendtextbox.ActualHeight*2), 0, 0);
            MenuOpen.Margin = new Thickness(0, 36, 0, 0);

        }
        async Task logout()
        {
            MenuOpen.Visibility = Visibility.Hidden;

            Logowanie.Visibility = Visibility.Visible;
            LoginBox.Visibility = Visibility.Visible;
            HasloBox.Visibility = Visibility.Visible;
            L2Haslo.Visibility = Visibility.Visible;
            l1Nazwa.Visibility = Visibility.Visible;


            ListClient.Visibility = Visibility.Hidden;

            zalogowano.Visibility = Visibility.Hidden;

            Logowanie.Visibility = Visibility.Visible;
            LoginBox.Visibility = Visibility.Visible;
            HasloBox.Visibility = Visibility.Visible;
            L2Haslo.Visibility = Visibility.Visible;
            l1Nazwa.Visibility = Visibility.Visible;

            chatbox.Visibility = Visibility.Hidden;
            Wyslijbtn.Visibility = Visibility.Hidden;
            sendtextbox.Visibility = Visibility.Hidden;
            
        }
        private async void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            await logout();
        }

        private void Label_MouseDown_1(object sender, MouseButtonEventArgs e)
        {

            chatbox1.Visibility = Visibility.Hidden;
            Wyslijbtn.Visibility = Visibility.Hidden;
            sendtextbox.Visibility = Visibility.Hidden;
        }

        private void MenuOpen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (logoutlabel.Visibility == Visibility.Visible)
            {
                logoutlabel.Visibility = Visibility.Hidden;
                closechatlabel.Visibility = Visibility.Hidden;
            }
            else
            {
                logoutlabel.Visibility = Visibility.Visible;
                closechatlabel.Visibility = Visibility.Visible;
            }
        }


        public void chidemainpanel()
        {
            MenuOpen.Visibility = Visibility.Hidden;

            Logowanie.Visibility = Visibility.Visible;
            LoginBox.Visibility = Visibility.Visible;
            HasloBox.Visibility = Visibility.Visible;
            L2Haslo.Visibility = Visibility.Visible;
            l1Nazwa.Visibility = Visibility.Visible;



            zalogowano.Visibility = Visibility.Hidden;

            chatbox.Visibility = Visibility.Hidden;
            Wyslijbtn.Visibility = Visibility.Hidden;
            sendtextbox.Visibility = Visibility.Hidden;
            

        }
        private async void Newchat_MouseDown(object sender, MouseButtonEventArgs e)
        {
            flagForChat = 2;

            userstochat.Clear();

            ListClient.Margin = new Thickness(0, 60, 0, 64);
            ListClient.Items.Clear();
            userslabel.Margin = new Thickness(this.ActualWidth - userslabel.Width, 0, 0, 0);
            userslabel.Visibility = Visibility.Visible;
            //https://localhost:7116/api/users/all
            response = await clientconnect.GetAsync($"https://{ip}:{port}/api/users/all");
            response.EnsureSuccessStatusCode();
            string jsonResponse2 = await response.Content.ReadAsStringAsync();
            var userData = JsonSerializer.Deserialize<List<UserForUser>>(jsonResponse2);
            foreach (var item in userData)
            {

                ListClient.Items.Add(item.name);
                userstochat.Add(item);


            }
        }

    }
}
