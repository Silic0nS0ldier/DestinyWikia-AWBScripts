	private static readonly Regex Infobox = Tools.NestedTemplateRegex(new List<string>("Infobox/Quest".Split(',')));
	public string ProcessArticle(string ArticleText, string ArticleTitle, int wikiNamespace, out string Summary, out bool Skip)
	{
		string OriginalArticleText = ArticleText;
		Summary = "";
		//Capture Data
		string
			image = "", 
			name = image,
            previous = image,
			next = previous,
			_class = next,
            questsource = _class
			faction = questsource,
			steps = faction,
            itemhash = steps;
			
			//Legacy strings
			string bosses = "";
		
		foreach (Match m in Infobox.Matches(ArticleText))
		{
			string InfoboxParameter = m.Value;
			
			image = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "image");
			name = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "name");
			previous = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "previous");
			next = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "next");
			_class = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "class");
            questsource = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "questsource");
			faction = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "faction");
            steps = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "steps");
			itemhash = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "itemhash");
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
		previous = previous.Trim();
		next = next.Trim();
		_class = _class.Trim();
        questsource = questsource.Trim();
		faction = faction.Trim();
        steps = steps.Trim();
		itemhash = itemhash.Trim();
		
		//Remove old infobox
		int InfoboxStart = ArticleText.IndexOf("{{Infobox/Quest"), InfoboxEnd = ArticleText.IndexOf("{{Infobox/Quest");
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
		
		DialogResult result = MessageBox.Show("Faction -> " + faction + "\nShould this be used in 'questsource'?", "ATTENTION", MessageBoxButtons.YesNo);
        if (result == DialogResult.Yes){
            questsource = faction;
        }
        
		//Create and add new infobox.
		///Create new infobox
		string newInfobox = 
			"{{Infobox/Quest\n|image=" + image + 
			"\n|name=" + name + 
			"\n|previous=" + previous + 
			"\n|next=" + next + 
			"\n|class=" + _class + 
            "\n|questsource=" + questsource + 
            "\n|steps=" + steps + 
			"\n|itemhash=" + itemhash + 
			"\n}}";
		///Split ArticleText
		string ArticleTextStart = ArticleText.Substring(0, InfoboxStart), ArticleTextEnd = ArticleText.Substring(InfoboxStart);
		///Assemble
		ArticleText = ArticleTextStart + newInfobox + ArticleTextEnd;
		
		
		Skip = false;
		
		return ArticleText;
	}