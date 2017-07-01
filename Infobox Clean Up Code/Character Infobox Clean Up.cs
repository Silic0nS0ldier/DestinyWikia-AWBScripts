	private static readonly Regex Infobox = Tools.NestedTemplateRegex(new List<string>("Infobox/Character".Split(',')));
	public string ProcessArticle(string ArticleText, string ArticleTitle, int wikiNamespace, out string Summary, out bool Skip)
	{
		string OriginalArticleText = ArticleText;
		Summary = "";
		//Capture Data
		string
			image = "", 
			name = image,
            title = image,
			symbol = title,
			race = symbol,
			gender = race,
			shop = gender,
            mission = shop,
			location = mission,
			map = location,
			actor = map,
			appearance = actor;
		
		foreach (Match m in Infobox.Matches(ArticleText))
		{
			string InfoboxParameter = m.Value;
			
			image = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "image");
			name = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "name");
			title = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "title");
			symbol = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "symbol");
			race = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "race");
			gender = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "gender");
            shop = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "shop");
			mission = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "mission");
			location = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "location");
			map = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "map");
			actor = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "actor");
			appearance = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "appearance");
		}
		
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
		///Icon
		if (map.Contains("[[File:"))
		{
			using (System.IO.StringReader readingMap = new System.IO.StringReader(map))
			{
				string mapCleaned = "";
				bool monitoredCharHit = false;
				do
				{
					char temp = Convert.ToChar(readingMap.Read());
					if (temp == ':')
					{
						monitoredCharHit = true;
					}
				}
				while (!monitoredCharHit);
				//read and save until "|" hit.
				while (Convert.ToChar(readingMap.Peek()) != '|' && readingMap.Peek() != -1 && Convert.ToChar(readingMap.Peek()) != ']')
				{
					mapCleaned += Convert.ToChar(readingMap.Read());
				}
				map = mapCleaned;
				readingMap.Close();
			}
		}
			
		//Trim the fat...
		image = image.Trim();
		name = name.Trim();
		title = title.Trim();
		symbol = symbol.Trim();
		race = race.Trim();
		gender = gender.Trim();
        shop = shop.Trim();
		mission = mission.Trim();
		location = location.Trim();
		map = map.Trim();
		actor = actor.Trim();
		appearance = appearance.Trim();
		

		
		//Remove old infobox
		int InfoboxStart = ArticleText.IndexOf("{{Infobox/Character"), InfoboxEnd = ArticleText.IndexOf("{{Infobox/Character");
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
			"{{Infobox/Character\n|image=" + image + 
			"\n|name=" + name + 
			"\n|title=" + title + 
			"\n|symbol=" + symbol + 
			"\n|race=" + race + 
			"\n|gender=" + gender + 
            "\n|shop=" + shop + 
			"\n|mission=" + mission + 
			"\n|location=" + location + 
			"\n|map=" + map + 
			"\n|actor=" + actor + 
			"\n|appearance=" + appearance + 
			"\n}}";
		///Split ArticleText
		string ArticleTextStart = ArticleText.Substring(0, InfoboxStart), ArticleTextEnd = ArticleText.Substring(InfoboxStart);
		///Assemble
		ArticleText = ArticleTextStart + newInfobox + ArticleTextEnd;
		
		
		Skip = false;
		
		return ArticleText;
	}