public class ValidationConstants
{
    public string NotEmptyMsg()
    {
        return "Required Field";
    }

    public string InvalidFormatMsg()
    {
        return "Invalid Format";
    }

    public string OnlyLettersPattern()
    {
        return @"^[a-zA-ZÀ-ÿ\s]+$";
    }

}