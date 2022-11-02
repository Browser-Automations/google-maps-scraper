using HtmlAgilityPack;
using PuppeteerSharp;

using var browserFetcher = new BrowserFetcher();
await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);

var browser = await Puppeteer.LaunchAsync(new LaunchOptions
{
    Headless = false
});

string url = "https://www.google.com/search?q=resturants&biw=1660&bih=841&tbm=lcl&ei=3uJiY57BJKqL9u8P54ehiA0&ved=0ahUKEwieqISSupD7AhWqhf0HHedDCNEQ4dUDCAk&uact=5&oq=resturants&gs_lcp=Cg1nd3Mtd2l6LWxvY2FsEAMyBwgAEMkDEEMyBQgAEJIDMgUIABCSAzIHCAAQsQMQQzIHCAAQsQMQQzIHCAAQgAQQCjIHCAAQgAQQCjIHCAAQgAQQCjIHCAAQgAQQCjIHCAAQgAQQCjoFCAAQkQI6CwgAEIAEELEDEIMBOggIABCxAxCDAToECAAQQzoFCAAQgARQAFiDDWDND2gAcAB4AIABpwKIAZEUkgEEMi0xMJgBAKABAcABAQ&sclient=gws-wiz-local";

var page = (await browser.PagesAsync())[0] ;
await page.GoToAsync(url);
await page.WaitForTimeoutAsync(10000);

HtmlDocument doc = new HtmlDocument();
doc.LoadHtml(await page.GetContentAsync());
var resultSection  = doc.DocumentNode.SelectSingleNode("//div[@id='search']");
if (resultSection == null)
{
    Console.WriteLine("Error => Results section not found");
}
else
{
    doc.LoadHtml(resultSection.InnerHtml);
    var listingSection = doc.DocumentNode.SelectSingleNode("/div/div/div/div/div/div/div/div/div[1]/div[3]").ChildNodes.Where(x=>!string.IsNullOrEmpty(x.InnerText));
    var items = listingSection.Take(22);
    Console.WriteLine("Total items on the page is "+items.Count());
    foreach (var item in items)
    {
        Console.WriteLine("=> "+item.InnerText.Substring(0,30));
    }
}
