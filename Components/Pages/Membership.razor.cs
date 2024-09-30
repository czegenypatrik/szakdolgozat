namespace Szakdolgozat.Components.Pages
{
    public partial class Membership
    {
        public string Text { get; set; } = "????";
        public string ButtonText { get; set; } = "Click Me";
        public int ButtonClicked { get; set; }
        public bool Dense_Radio { get; set; } = true;

        void ButtonOnClick()
        {
            ButtonClicked += 1;
            Text = $"Awesome x {ButtonClicked}";
            ButtonText = "Click Me Again";
        }
    }
}
