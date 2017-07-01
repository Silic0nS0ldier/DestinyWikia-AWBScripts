private static readonly Regex Infobox = Tools.NestedTemplateRegex(new List<string>("Infobox/Armor".Split(',')));
public string ProcessArticle(string ArticleText, string ArticleTitle, int wikiNamespace, out string Summary, out bool Skip)
{
    Skip = true;
    Summary = "";
    string OriginalArticleText = ArticleText;
    //Capture Data
    string
        image = "",
        name = image,
        type = name,
        _class = type,
        rarity = _class,
        manufacturer = rarity,
        armorset = manufacturer,
        level = armorset,
        defense = level,
        discipline = defense,
        strength = discipline,
        intellect = strength,
        quality = intellect,
        tier = quality,
        icon = tier,
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
        type = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "type");
        _class = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "class");
        rarity = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "rarity");
        manufacturer = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "manufacturer");
        armorset = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "armorset");
        level = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "level");
        defense = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "defense");
        discipline = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "discipline");
        strength = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "strength");
        intellect = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "intellect");
        quality = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "quality");
        tier = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "tier");
        icon = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "icon");
        stack = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "stack");
        transfer = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "transfer");
        action = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "action");
        loot = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "loot");
        vendor = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "vendor");
        price = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "price");
        itemhash = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "itemhash");
    }

    //Send icon in image to icon.
    if (image.Contains("icon.jpg"))
    {
        icon = image;
        image = "";
        Skip = false;
    }

    //Trim the fat...
    image = image.Trim();
    name = name.Trim();
    type = type.Trim();
    _class = _class.Trim();
    rarity = rarity.Trim();
    manufacturer = manufacturer.Trim();
    armorset = armorset.Trim();
    level = level.Trim();
    defense = defense.Trim();
    discipline = discipline.Trim();
    strength = strength.Trim();
    intellect = intellect.Trim();
    quality = quality.Trim();
    tier = tier.Trim();
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
        Skip = false;
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
        Skip = false;
    }

    //Remove old infobox
    int InfoboxStart = ArticleText.IndexOf("{{Infobox/Armor");
    string oldTemplate = WikiFunctions.Parse.Parsers.GetTemplate(ArticleText, "Infobox/Armor");
    ArticleText = ArticleText.Remove(InfoboxStart, oldTemplate.Length);


    //Create and add new infobox.
    ///Create new infobox
    string newInfobox =
        "{{Infobox/Armor\n|image=" + image +
        "\n|name=" + name +
        "\n|type=" + type +
        "\n|class=" + _class +
        "\n|rarity=" + rarity +
        "\n|manufacturer=" + manufacturer +
        "\n|armorset=" + armorset +
        "\n|level=" + level +
        "\n|defense=" + defense +
        "\n|discipline=" + discipline +
        "\n|strength=" + strength +
        "\n|intellect=" + intellect +
        "\n|quality=" + quality +
        "\n|tier=" + tier +
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

    //Check if screenshot avaliable, and check for existing image template.
    if (image.Length == 0 && WikiFunctions.Parse.Parsers.GetTemplate(ArticleText, "Images") == "" && !ArticleText.Contains("Deleted Material"))
    {
        string temp = "";
        if (InfoboxStart == 0)
        {
            temp = "\n";
        }
        ArticleText = "{{Images|6 August 2016|itemtype=Armor}}" + temp + ArticleText;
        Summary += "Flagged for images";
        Skip = false;
    }

    var temp1 = ArticleText;
    ArticleText = ArticleText.Replace("[[Warlock Bond]]", "[[Bond]]");
    ArticleText = ArticleText.Replace("[[Hunter Cloak]]", "[[Cloak]]");
    ArticleText = ArticleText.Replace("[[Titan Mark]]", "[[Mark]]");

    if (temp1 != ArticleText)
    {
        Skip = false;
    }

    return ArticleText;
}