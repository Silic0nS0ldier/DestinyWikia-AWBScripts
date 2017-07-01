	private static readonly Regex Infobox = Tools.NestedTemplateRegex(new List<string>("Infobox/Sparrow".Split(',')));
	public string ProcessArticle(string ArticleText, string ArticleTitle, int wikiNamespace, out string Summary, out bool Skip)
	{
		Summary = "";
		//Capture Data
		string
			image = "", 
			name = image,
			_class = name,
			rarity = _class,
			manufacturer = rarity,
			speed = manufacturer,
			boost = speed,
			durability = boost,
            icon = durability
			stack = icon,
			transfer = stack,
			action = transfer,
			loot = action,
			vendor = loot,
			price = vendor,
			itemhash = price;
		
		foreach (Match m in Infobox.Matches(ArticleText))
		{
			string InfoboxParameter = m.Value;
			
			image = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "image");
			name = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "name");
			_class = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "class");
			rarity = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "rarity");
			manufacturer = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "manufacturer");
			speed = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "speed");
			boost = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "boost");
			durability = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "durability");
			icon = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "icon");
			stack = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "stack");
			transfer = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "transfer");
			action = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "action");
			loot = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "loot");
			vendor = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "vendor");
			price = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "price");
			itemhash = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "itemhash");
		}
		
		//Send icon to icon parameter is hidden inside comment.
		if (image.Contains("<!--[[File:"))
		{
			int start = image.IndexOf("<!--[[File:"), end = image.IndexOf("]]-->");
			icon = image.Substring(start + 2, end - start);
			//Remove icon from image parameter.
			image = image.Substring(0, start);
		}
		
		//Send icon in image to icon.
		if (image.Contains("icon") || image.Contains("Icon"))
		{
			icon = image;
			image = "";
		}
		
		//Trim the fat...
		image = image.Trim();
		name = name.Trim();
		type = type.Trim();
		_class = _class.Trim();
		rarity = rarity.Trim();
		manufacturer = manufacturer.Trim();
		speed = speed.Trim();
		boost = boost.Trim();
		durability = durability.Trim();
		icon = icon.Trim();
		stack = stack.Trim();
		transfer = transfer.Trim();
		action = action.Trim();
		loot = loot.Trim();
		vendor = vendor.Trim();
		price = price.Trim();
		itemhash = itemhash.Trim();
		
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
		if (icon.Contains("[[File:"))
		{
			using (System.IO.StringReader readingIcon = new System.IO.StringReader(icon))
			{
				string iconCleaned = "";
				bool monitoredCharHit = false;
				do
				{
					char temp = Convert.ToChar(readingIcon.Read());
					if (temp == ':')
					{
						monitoredCharHit = true;
					}
				}
				while (!monitoredCharHit);
				//read and save until "|" hit.
				while (Convert.ToChar(readingIcon.Peek()) != '|' && readingIcon.Peek() != -1 && Convert.ToChar(readingIcon.Peek()) != ']')
				{
					iconCleaned += Convert.ToChar(readingIcon.Read());
				}
				icon = iconCleaned;
				readingIcon.Close();
			}
		}
		
		//Remove old infobox
		int InfoboxStart = ArticleText.IndexOf("{{Infobox/Sparrow"), InfoboxEnd = ArticleText.IndexOf("{{Infobox/Sparrow");
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
			"{{Infobox/Sparrow\n|image=" + image + 
			"\n|name=" + name + 
			"\n|type=" + type + 
			"\n|class=" + _class + 
			"\n|rarity=" + rarity + 
			"\n|manufacturer=" + manufacturer + 
			"\n|speed=" + speed + 
			"\n|boost=" + boost + 
			"\n|durability=" + durability + 
			"\n|icon=" + icon + 
			"\n|stack=" + stack + 
			"\n|transfer=" + transfer + 
			"\n|action=" + action + 
			"\n|loot=" + loot + 
			"\n|vendor=" + vendor + 
			"\n|price=" + price + 
			"\n|itemhash=" + itemhash +
			"\n}}";
		///Split ArticleText
		string ArticleTextStart = ArticleText.Substring(0, InfoboxStart), ArticleTextEnd = ArticleText.Substring(InfoboxStart);
		///Assemble
		ArticleText = ArticleTextStart + newInfobox + ArticleTextEnd;
		
		
		Skip = false;
		return ArticleText;
	}