namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var label = new Label { Text = "Enter name:", Top = 20, Left = 10 };
            var textbox = new TextBox { Name = "NameInput", Top = 45, Left = 10, Width = 200 };
            var button = new Button { Name = "GreetButton", Text = "Greet", Top = 80, Left = 10 };
            var resultLabel = new Label { Name = "ResultLabel", Top = 115, Left = 10, Width = 300 };

            button.Click += (s, e) =>
            {
                resultLabel.Text = $"Hello, {textbox.Text}!";
            };

            Controls.Add(label);
            Controls.Add(textbox);
            Controls.Add(button);
            Controls.Add(resultLabel);
        }
    }
}
