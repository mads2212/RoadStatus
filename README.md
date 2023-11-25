# RoadStatus

Instructions to run: 
- Just build the app and run the .exe file under bin\Debug\net6.0
- Don't forget to configure the API keys and appId on appsettings.json. There are two in the solution, one for the Console App (RoadStatus) and the other for the Tests (RoadStatus.Tests)
- The tests were created with NUnit. Use your favourite way of running them. I run them through my IDE (Jetbrains Rider).


OBS: The endpoinds provided by you in the examples on the coding challenge instructions document worked with any value for the key and the app id.

https://api.tfl.gov.uk/Road/A2?app_id=your_app_id&amp;app_key=your_developer_key <- This actually works and give valid results

I'm not sure if this was inteded or not as part of the challenge so I used what was actually in the Tfl Unified API website and that work as expected, requiring valid API keys:

https://api.tfl.gov.uk/Road/A2/Status?app_id=VALIDAPPID&app_key=VALIDAPIKEY

Both of them work without passing keys though.
