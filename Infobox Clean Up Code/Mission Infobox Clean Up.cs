	private static readonly Regex Infobox = Tools.NestedTemplateRegex(new List<string>("Infobox/Mission".Split(',')));
	public string ProcessArticle(string ArticleText, string ArticleTitle, int wikiNamespace, out string Summary, out bool Skip)
	{
		string OriginalArticleText = ArticleText;
		Summary = "";
		//Capture Data
		string
			image = "", 
			name = image,
            		location = image,
			type = location,
			required = type,
			unlocks = required,
			majors = unlocks,
            		ultras = majors,
			difficulties = ultras,
			rewards = difficulties;
			
			//Legacy strings
			string bosses = "";
		
		foreach (Match m in Infobox.Matches(ArticleText))
		{
			string InfoboxParameter = m.Value;
			
			image = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "image");
			name = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "name");
			location = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "location");
			type = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "type");
			required = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "required");
			unlocks = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "unlocks");
            		majors = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "majors");
			ultras = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "ultras");
			difficulties = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "difficulties");
			rewards = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "rewards");
			
			//Legacy extraction
			bosses = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "bosses");
		}
		
		bosses = bosses.Trim();
		
		if (bosses != "")
		{
			System.Windows.Forms.MessageBox.Show(bosses);
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
		type = type.Trim();
		required = required.Trim();
		unlocks = unlocks.Trim();
        	majors = majors.Trim();
		ultras = ultras.Trim();
		difficulties = difficulties.Trim();
		rewards = rewards.Trim();
		
		//Remove old infobox
		int InfoboxStart = ArticleText.IndexOf("{{Infobox/Mission"), InfoboxEnd = ArticleText.IndexOf("{{Infobox/Mission");
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
			"{{Infobox/Mission\n|image=" + image + 
			"\n|name=" + name + 
			"\n|location=" + location + 
			"\n|type=" + type + 
			"\n|required=" + required + 
            		"\n|unlocks=" + unlocks + 
            		"\n|majors=" + majors + 
			"\n|ultras=" + ultras + 
			"\n|difficulties=" + difficulties + 
			"\n|rewards=" + rewards + 
			"\n}}";
		///Split ArticleText
		string ArticleTextStart = ArticleText.Substring(0, InfoboxStart), ArticleTextEnd = ArticleText.Substring(InfoboxStart);
		///Assemble
		ArticleText = ArticleTextStart + newInfobox + ArticleTextEnd;
		
		
		Skip = false;
		
		return ArticleText;
	}