private static readonly Regex Infobox = Tools.NestedTemplateRegex(new List<string>("Infobox/Weapon".Split(',')));
public string ProcessArticle(string ArticleText, string ArticleTitle, int wikiNamespace, out string Summary, out bool Skip)
{
    Skip = true;
    string OriginalArticleText = ArticleText;
    Summary = "";
    //Capture Data
    string
        image = "",
        name = image,
        type = name,
        slot = type,
        rarity = slot,
        manufacturer = rarity,
        level = manufacturer,
        damage = level,
        speed = damage,
        impact = speed,
        range = impact,
        blastradius = range,
        velocity = blastradius,
        efficiency = velocity,
        defense = efficiency,
        energy = defense,
        zoom = energy,
        aim = zoom,
        stability = aim,
        recoil = stability,
        equipspeed = recoil,
        magazine = equipspeed,
        rate = magazine,
        reload = rate,
        quality = reload,
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
        slot = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "slot");
        rarity = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "rarity");
        manufacturer = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "manufacturer");
        level = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "level");
        damage = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "damage");
        speed = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "speed");
        impact = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "impact");
        range = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "range");
        blastradius = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "blastradius");
        velocity = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "velocity");
        efficiency = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "efficiency");
        defense = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "defense");
        energy = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "energy");
        zoom = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "zoom");
        aim = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "aim");
        stability = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "stability");
        recoil = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "recoil");
        equipspeed = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "equipspeed");
        magazine = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "magazine");
        rate = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "rate");
        reload = WikiFunctions.Tools.GetTemplateParameterValue(InfoboxParameter, "reload");
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
    slot = slot.Trim();
    rarity = rarity.Trim();
    manufacturer = manufacturer.Trim();
    level = level.Trim();
    damage = damage.Trim();
    speed = speed.Trim();
    impact = impact.Trim();
    range = range.Trim();
    blastradius = blastradius.Trim();
    velocity = velocity.Trim();
    efficiency = efficiency.Trim();
    defense = defense.Trim();
    energy = energy.Trim();
    zoom = zoom.Trim();
    aim = aim.Trim();
    stability = stability.Trim();
    recoil = recoil.Trim();
    equipspeed = equipspeed.Trim();
    magazine = magazine.Trim();
    rate = rate.Trim();
    reload = reload.Trim();
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

    //Find old template position and remove old template
    int InfoboxStart = ArticleText.IndexOf("{{Infobox/Weapon");
    string oldTemplate = WikiFunctions.Parse.Parsers.GetTemplate(ArticleText, "Infobox/Weapon");
    ArticleText = ArticleText.Remove(InfoboxStart, oldTemplate.Length);



    //Create and add new infobox.
    ///Create new infobox
    string newInfobox =
        "{{Infobox/Weapon\n|image=" + image +
        "\n|name=" + name +
        "\n|type=" + type +
        "\n|slot=" + slot +
        "\n|rarity=" + rarity +
        "\n|manufacturer=" + manufacturer +
        "\n|level=" + level +
        "\n|damage=" + damage +
        "\n|speed=" + speed +
        "\n|impact=" + impact +
        "\n|range=" + range +
        "\n|blastradius=" + blastradius +
        "\n|velocity=" + velocity +
        "\n|efficiency=" + efficiency +
        "\n|defense=" + defense +
        "\n|energy=" + energy +
        "\n|zoom=" + zoom +
        "\n|aim=" + aim +
        "\n|stability=" + stability +
        "\n|recoil=" + recoil +
        "\n|equipspeed=" + equipspeed +
        "\n|magazine=" + magazine +
        "\n|rate=" + rate +
        "\n|reload=" + reload +
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
        ArticleText = "{{Images|30 July 2016|itemtype=Weapon}}" + temp + ArticleText;
        Summary += "Flagged for images";
        Skip = false;
    }

    return ArticleText;
}