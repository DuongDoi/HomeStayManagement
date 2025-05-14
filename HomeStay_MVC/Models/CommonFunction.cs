namespace HomeStay_MVC.Model
{
    public class CommonFunction
    {

        public static string GetValuesAppSetting(string key, string subkey)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

            var root = builder.Build();
            var sampleConnectionString = root.GetSection(key)[subkey];
            return sampleConnectionString;
        }



    }
}
