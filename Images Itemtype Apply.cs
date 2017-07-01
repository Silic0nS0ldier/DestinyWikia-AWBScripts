	private static readonly Regex Infobox = Tools.NestedTemplateRegex(new List<string>("Infobox/PvPmap".Split(',')));
	public string ProcessArticle(string ArticleText, string ArticleTitle, int wikiNamespace, out string Summary, out bool Skip)
	{
		string OriginalArticleText = ArticleText;
		Summary = "";
		
		//Remove old images
		int TemplateStart = ArticleText.IndexOf("{{Images"), TemplateEnd = TemplateStart;
		if (TemplateStart == -1)
		{
			TemplateStart = ArticleText.IndexOf("{{images");
			TemplateEnd = TemplateStart;
		}
		using (System.IO.StringReader readingArticleText = new System.IO.StringReader(ArticleText))
		{
			//Read to infobox.
			int index = 0;
			do
			{
				readingArticleText.Read();
				index++;
			}
			while (index < TemplateStart);
			
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
				TemplateEnd++;
				readingArticleText.Read();
			}
			while (incompleteCurleyBraketPairs != 0);
			ArticleText = ArticleText.Remove(TemplateStart, TemplateEnd - TemplateStart);//Sometimes fail.
			//Clean up
			if (TemplateStart == ArticleText.IndexOf("}}"))
			{
				ArticleText = ArticleText.Remove(TemplateStart, 2);
			}
		}
		
		
		//Create and add new infobox.
		///Create new infobox
		string NewTemplate = 
			"{{Images|29 February 2016|itemtype=Shader}}";

		//Add to start.
		ArticleText = NewTemplate + ArticleText;
		Skip = false;
		
		return ArticleText;
	}