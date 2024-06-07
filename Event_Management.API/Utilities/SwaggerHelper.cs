using SwaggerThemes;

namespace Event_Management.API.Utilities
{
	public class SwaggerHelper
	{
		public static Theme GetTheme()
		{
			Theme[] themes = new Theme[]
			{
				Theme.XCodeLight,
				Theme.Dracula,
				Theme.Gruvbox,
				Theme.OneDark,
				Theme.Monokai,
				Theme.Sepia,
				Theme.UniversalDark,
				Theme.NordDark
			};

			Random random = new Random();
			int index = random.Next(themes.Length);
			return themes[index];
		}
	}
}
