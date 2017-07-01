	private static readonly Regex Infobox = Tools.NestedTemplateRegex(new List<string>("Infobox/Class".Split(',')));
	public string ProcessArticle(string ArticleText, string ArticleTitle, int wikiNamespace, out string Summary, out bool Skip)
	{
		string OriginalArticleText = ArticleText;
		Summary = "";
		//Capture Data
		string
			image = "", 
			name = image,
			subclasses = name,
			classarmor = subclasses,
			melee = classarmor,
			specialties = melee;
		
		foreach (Match m in Infobox.Matches(ArticleText))
		{
			string InfoboxParameter = m.Value;
			
			image = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "image");
			name = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "class_name");
			subclasses = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "subclasses");
			classarmor = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "class_armor");
			melee = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "class_melee");
			specialties = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "specialties");
		}
		
		//Trim the fat...
		image = image.Trim();
		name = name.Trim();
		subclasses = subclasses.Trim();
		classarmor = classarmor.Trim();
		melee = melee.Trim();
		specialties = specialties.Trim();
		
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
		int InfoboxStart = ArticleText.IndexOf("{{Infobox/Class"), InfoboxEnd = ArticleText.IndexOf("{{Infobox/Class");
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
			"{{Infobox/Class\n|image=" + image + 
			"\n|name=" + name +  
			"\n|subclasses=" + subclasses + 
			"\n|classarmor=" + classarmor + 
			"\n|melee=" + melee + 
            "\n|specialties=" + specialties + 
			"\n}}";
		///Split ArticleText
		string ArticleTextStart = ArticleText.Substring(0, InfoboxStart), ArticleTextEnd = ArticleText.Substring(InfoboxStart);
		///Assemble
		ArticleText = ArticleTextStart + newInfobox + ArticleTextEnd;
		
		
		Skip = false;
		
		return ArticleText;
	}