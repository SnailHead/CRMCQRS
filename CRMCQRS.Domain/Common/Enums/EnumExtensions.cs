using System.ComponentModel.DataAnnotations;

namespace CRMCQRS.Domain.Common.Enums;

public static class EnumExtensions
{
	public static string GetDisplayName<T>(this T value) where T: Enum
	{
		return value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false)
			.Cast<DisplayAttribute>().FirstOrDefault()?.GetName() ?? value.ToString();
	}
}