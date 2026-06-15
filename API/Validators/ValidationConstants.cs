public class ValidationConstants
{
    public string NotEmptyMsg()
    {
        return "Campo obrigatorio";
    }

    public string InvalidFormatMsg()
    {
        return "Formato Invalido";
    }

    public string OnlyLettersPattern()
    {
        return @"^[a-zA-ZÀ-ÿ\s]+$";
    }

}