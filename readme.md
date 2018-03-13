# Coffee GIF Chatbot
This is an Azure Bot Framework sample that uses LUIS and the giphy API to return a random G-rated coffee gif when you ask for it. 


# Creating your own
Creating your own chatbot is really easy. We'll use a Web App Bot, hosted in an Azure app service, so you can deploy code to it from Visual Studio. The chatbot is just a WebApi project with Bot Framework dialogs and LUIS integration. 

## Create the chat bot in Azure
1. Create a new resource group
2. Add a new Web App Bot resource. You will need to specify an App Service Plan and storage account, or create new ones.
3. For **Bot Template**, choose **Language Understanding**. This will also create a LUIS cognitive services resource, app, and API Key. Once this resource is created, you can access and train your LUIS model at https://www.luis.ai. 
4. If you want App Insights, make sure to choose a region.
5. Check the box agreeing to LUIS terms and create the resource. 
6. When the resource is done, go to the chat bot you created (the Web App Bot) and go to the **Build** blade. Download your bot code as a zip, extract it, and make a repo if you wish (or clone this one). You can also edit your bot directly online, but it works a little better in Visual Studio with the Publish workflow.

**IMPORTANT: You must add the AzureWebJobsStorage key to your Web.config or you will get an exception when testing locally. You can find the value of this key in the Azure properties of your bot.**

You can put all of your key values in Web.config if you want. This repository uses a static class so that the API keys can be hidden on Github but still usable.

### Testing your brand new LUIS bot
Once these resources are created, you can test the chat bot in Web Chat by navigating to your Web App Bot resource in Azure and clicking **Test in Web Chat**. Try sending your bot some messages such as *hello, hi, help,* or *cancel*. Next, we'll extend what the chatbot is capable of. 

## Train your language model at luis.ai
1. Navigate to https://www.luis.ai and sign in with your Azure credentials.
2. Click **My Apps** and click your newly created app. **Make sure to capture your App ID and LUIS API Key.**
3. Click **Intents**. The default template contains four intents: None, Help, Greeting, and Cancel.
4. **Create a new intent** called Brew and click Done. 
5. Click on the Brew intent. Time to start training!
6. Type 5-10 examples of how you expect users to ask your bot for coffee. For example, `I'd like a cup of joe`, `Brew me some coffee, please` or `Get me some caffeine, you bot!` Each of these phrases is called an *utterance*.
7. Start tagging these utterances. As you mouse over words of the captured utterance, you can click the word and create an **Entity**. In this case, let's tag all the words like *coffee, joe, caffeine* with an entity we create called `Coffee`. We can also tag words like *brew, make, percolate* with another entity called `Brew`. 
8. When you're done tagging your utterances, train the model by clicking **Train**.
9. **Publish** your model. 

## Handle coffee-related requests
In `Dialogs/BasicLuisDialog.cs` see the `Brew` intent. You can add code here to simply post a message back to the user at first, like "Here's your coffee."

You can create more of these async methods and tag them accordingly with attributes to handle any intent you've trained in your LUIS model. 
[BasicLuisDialog.cs](https://github.com/danwaters/CoffeeGifChatbot/blob/b3f13f8fab0306baa479457faa2fc5f263a53fa6/Dialogs/BasicLuisDialog.cs#L48)

## Grabbing a random GIF
[Giphy](https://www.giphy.com) has a developer API you can sign up for which has a random GIF feature. Sign up for an API key and use the code in [GifService.cs](https://github.com/danwaters/CoffeeGifChatbot/blob/master/Services/GifService.cs) to connect to it. Make sure to put your API key in Keys.cs. 

```
public async Task<CoffeeGif> GetRandomCoffeeGif()
{
    using (var client = new HttpClient())
    {
        var result = await client.GetAsync(serviceUrl);
        var content = await result.Content.ReadAsStringAsync();
        JObject obj = JObject.Parse(content);
        CoffeeGif reply = new CoffeeGif
        {
            GiphyWebUrl = obj["data"]["bitly_gif_url"].ToString(),
            GifUrl = obj["data"]["images"]["fixed_width"]["url"].ToString()
        };
        return reply;
    }
}
```

This service is then used in BasicLuisDialog.cs in the intent handler for `Brew`.

## Attach an image to the message
The following code in `BasicLuisDialog.cs` (Brew intent handler) will attach the gif to the message back to the user requesting coffee. 

# Testing your bot locally
Run your project from Visual Studio. It will launch a browser that points to something like `localhost:3987` (port may vary). You can then connect to your bot with the [Bot Framework Emulator](https://github.com/Microsoft/BotFramework-Emulator/releases). 

Now, when you ask your bot for coffee, you'll get a sassy gif!
