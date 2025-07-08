namespace PetFamily.Domain.Shared
{
    public class Constants
    {
        public const int MAX_NAME_LENGTH = 100;
        public const int MAX_TEXT_DESCRIPTION_LENGTH = 2000;
        public const int MAX_PHONE_NUMBER_LENGTH = 20;
        public const int MAX_LINK_LENGTH = 750;
        public const int MAX_EMAIL_LENGTH = 320;
        public const int MAX_YEARS_OF_EXPERIENCE = 100;
        public const int MAX_DOMASTIC_PET_AGE = 25;
        public const int MAX_DOMASTIC_PET_WEIGHT_KG = 150;
        public const int MAX_DOMASTIC_PET_HEIGHT_SM = 200;
        public const int MAX_EXOTIC_PET_AGE = 200;

        public const int MAX_FILE_SIZE = 30 * 1024 * 1024; // 30 MB
        public const string PHOTO_FILE_EXTENSION = ".jpg";

        public const string REGEX_FULLNAME_PATTERN =
            @"^(?:(?:[А-ЯЁ][а-яё]+(?:-[А-ЯЁ][а-яё]+)?\s){1,2}[А-ЯЁ][а-яё]+(?:-[А-ЯЁ][а-яё]+)?|(?:[A-Z][a-z]+(?:-[A-Z][a-z]+)?\s){1,2}[A-Z][a-z]+(?:-[A-Z][a-z]+)?)$";

        public const string REGEX_EMAIL_PATTERN =
            @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";

        public const string REGEX_PHONE_NUMBER_PATTERN =
            @"^\+?[0-9\s\-\(\)]{7,20}$";

    }
}
