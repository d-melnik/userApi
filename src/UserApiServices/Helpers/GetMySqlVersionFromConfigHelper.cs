namespace UserApiServices.Helpers;

public static class GetMySqlVersionFromConfigHelper
{
    public const string ErrorMessage =
        "MySQL version should be specified in app.settings in format <int>.<string>.<string>, e.g. 8.0.1";
    public static Version GetVersion(string version)
    {
        if (string.IsNullOrEmpty(version))
        {
            throw new ArgumentNullException(ErrorMessage);
        }

        string[] splitVersionStr = version.Split(".");
        if (splitVersionStr.Length != 3)
        {
            throw new ArgumentException(ErrorMessage);
        }

        int[] splitVersionInt = new int[3];
        for (int i =0; i < 3 ; i++)
        {
            if (!int.TryParse(splitVersionStr[i], out splitVersionInt[i]))
            {
                throw new ArgumentException($"Value on position {i} in version is not int.{Environment.NewLine}{ErrorMessage}");
            }
        }
        return new Version(splitVersionInt[0],splitVersionInt[1],splitVersionInt[2]);
    }
}