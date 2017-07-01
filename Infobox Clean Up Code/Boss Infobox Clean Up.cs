	private static readonly Regex Infobox = Tools.NestedTemplateRegex(new List<string>("Infobox/Boss".Split(',')));
	public string ProcessArticle(string ArticleText, string ArticleTitle, int wikiNamespace, out string Summary, out bool Skip)
	{
		string OriginalArticleText = ArticleText;
		Summary = "";
		//Capture Data
		string
			image = "", 
			name = image,
			race = name,
			rank = race,
			mission = rank,
			location = mission,
            actor = location,
			appearance = actor;
		
		foreach (Match m in Infobox.Matches(ArticleText))
		{
			string InfoboxParameter = m.Value;
			
			image = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "image");
			name = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "name");
			race = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "race");
			rank = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "rank");
			mission = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "mission");
			location = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "location");
            actor = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "actor");
			appearance = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "appearance");
		}
		
		//Trim the fat...
		image = image.Trim();
		name = name.Trim();
		race = race.Trim();
		rank = rank.Trim();
		mission = mission.Trim();
		location = location.Trim();
        actor = actor.Trim();
		appearance = appearance.Trim();
		
		//Clean up where possible...
		///Image
		if (image.Contains("[[File:"))
		{
			using (System.IO.StringReader readingImage = new System.IO.StringReader(image))
			{
				string imageCleaned = "";
				bool monitoredCharHit = false;
				do
				{
					char temp = Convert.ToChar(readingImage.Read());
					if (temp == ':')
					{
						monitoredCharHit = true;
					}
				}
				while (!monitoredCharHit);
				//read and save until "|" hit.
				while (Convert.ToChar(readingImage.Peek()) != '|' && readingImage.Peek() != -1 && Convert.ToChar(readingImage.Peek()) != ']')
				{
					imageCleaned += Convert.ToChar(readingImage.Read());
				}
				image = imageCleaned;
				readingImage.Close();
			}
		}
		
		//Remove old infobox
		int InfoboxStart = ArticleText.IndexOf("{{Infobox/Boss"), InfoboxEnd = ArticleText.IndexOf("{{Infobox/Boss");
		using (System.IO.StringReader readingArticleText = new System.IO.StringReader(ArticleText))
		{
			//Read to infobox.
			int index = 0;
			do
			{
				readingArticleText.Read();
				index++;
			}
			while (index < InfoboxStart);
			
			//Find end
			int incompleteCurleyBraketPairs = 0;
			do
			{
				if (Convert.ToChar(readingArticleText.Peek()) == '{')
				{
					incompleteCurleyBraketPairs++;
				}
				else if (Convert.ToChar(readingArticleText.Peek()) == '}')
				{
					incompleteCurleyBraketPairs--;
				}
				InfoboxEnd++;
				readingArticleText.Read();
			}
			while (incompleteCurleyBraketPairs != 0);
			ArticleText = ArticleText.Remove(InfoboxStart, InfoboxEnd - InfoboxStart);//Sometimes fail.
			//Clean up
			if (InfoboxStart == ArticleText.IndexOf("}}"))
			{
				ArticleText = ArticleText.Remove(InfoboxStart, 2);
			}
		}
		
		
		//Create and add new infobox.
		///Create new infobox
		string newInfobox = 
			"{{Infobox/Boss\n|image=" + image + 
			"\n|name=" + name + 
			"\n|race=" + race + 
			"\n|rank=" + rank + 
			"\n|mission=" + mission + 
			"\n|location=" + location + 
            		"\n|actor =" + actor + 
			"\n|appearance=" + appearance + 
			"\n}}";
		///Split ArticleText
		string ArticleTextStart = ArticleText.Substring(0, InfoboxStart), ArticleTextEnd = ArticleText.Substring(InfoboxStart);
		///Assemble
		ArticleText = ArticleTextStart + newInfobox + ArticleTextEnd;
		
		
		Skip = false;
		
		return ArticleText;
	}