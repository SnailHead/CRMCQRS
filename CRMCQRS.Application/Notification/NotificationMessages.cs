namespace CRMCQRS.Application.Notification;

public static class NotificationMessages
{
    public const string ErrorFromGet = "Произошла ошибка при получение данных";
    public const string ErrorFromDelete = "Произошла ошибка при удаление данных";
    public const string ErrorFromUpdate = "Произошла ошибка при изменение данных";
    public const string ErrorFromCreate = "Произошла ошибка при создание данных";
    public const string SuccessDelete = "Удаление произошло успешно";
    public const string SuccessUpdate = "Обновление произошло успешно";
    public const string SuccessCreate = "Добавление произошло успешно";
    public const string FormValidationFail = "Данные введены не корректно";
}