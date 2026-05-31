namespace NutriSnap
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddRecordPage), typeof(AddRecordPage));
        }
    }
}
