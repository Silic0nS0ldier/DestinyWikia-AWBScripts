	private static readonly Regex Infobox = Tools.NestedTemplateRegex(new List<string>("Infobox/PvPmap".Split(',')));
	public string ProcessArticle(string ArticleText, string ArticleTitle, int wikiNamespace, out string Summary, out bool Skip)
	{
		string OriginalArticleText = ArticleText;
		Summary = "";
		//Capture Data
		string
			image = "", 
			name = image,
            location = image,
			terrain = location,
			visibility = terrain,
			light = visibility,
			size = light,
            players = size,
            heavyammo = players,
            turrents = heavyammo,
            vehicles = turrents,
            gametypes = vehicles;
		
		foreach (Match m in Infobox.Matches(ArticleText))
		{
			string InfoboxParameter = m.Value;
			
			image = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "image");
			name = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "name");
			location = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "location");
			terrain = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "terrain");
			visibility = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "visibility");
			light = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "light");
            size = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "size");
			players = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "num_players");
            heavyammo = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "num_of_heavy_ammo_drops");
            vehicles = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "vehicles");
            gametypes = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "gametypes");
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
			
		//Trim the fat...
		image = image.Trim();
		name = name.Trim();
		location = location.Trim();
		terrain = terrain.Trim();
		visibility = visibility.Trim();
		light = light.Trim();
        size = size.Trim();
		players = players.Trim();
        heavyammo = heavyammo.Trim();
        vehicles = vehicles.Trim();
        gametypes = gametypes.Trim();
		
		//Remove old infobox
		int InfoboxStart = ArticleText.IndexOf("{{Infobox/PvPmap"), InfoboxEnd = ArticleText.IndexOf("{{Infobox/PvPmap");
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
			"{{Infobox/PvPmap\n|image=" + image + 
			"\n|name=" + name + 
			"\n|location=" + location + 
			"\n|terrain=" + terrain + 
			"\n|visibility=" + visibility + 
            "\n|light=" + light + 
            "\n|size=" + size + 
			"\n|players=" + players + 
            "\n|heavyammo=" + heavyammo + 
            "\n|vehicles=" + vehicles + 
            "\n|gametypes=" + gametypes + 
			"\n}}";
		///Split ArticleText
		string ArticleTextStart = ArticleText.Substring(0, InfoboxStart), ArticleTextEnd = ArticleText.Substring(InfoboxStart);
		///Assemble
		ArticleText = ArticleTextStart + newInfobox + ArticleTextEnd;
		
		
		Skip = false;
		
		return ArticleText;
	}