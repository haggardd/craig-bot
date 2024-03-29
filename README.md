[![Build Status][build-shield]][build-url]
[![MIT License][license-shield]][license-url]

[build-shield]: https://github.com/haggardd/craig-bot/workflows/.NET%20Core/badge.svg
[build-url]: https://github.com/haggardd/craig-bot/actions?query=workflow%3A%22.NET+Core%22
[license-shield]: https://img.shields.io/github/license/haggardd/craig-bot.svg?style=flat
[license-url]: LICENSE

<div align="center">
  <a href="https://github.com/haggardd/craig-bot">
    <img src="logo.png" alt="Logo" width="256" height="256">
  </a>
  <h3 align="center">
  	Craig Bot
  </h3>
  <p align="center">
  	A configurable multi-purpose Discord bot named Craig, built for self hosting on small guilds.
  </p>
</div>

## Installation

First things first, you'll need to create a bot via the Discord Developer Portal, a guide can be found [HERE](https://discord.foxbot.me/docs/guides/getting_started/first-bot.html).

### Running Craig

1. Clone the repo or download it from GitHub and unzip it

   ```sh
   git clone https://github.com/haggardd/craig-bot.git
   ```

2. Publish Craig to a folder

   ```sh
   cd craig-bot
   dotnet publish -o [YOUR-OUTPUT-DIRECTORY]
   ```

3. Create a `config.json` file within your output directory with its contents reflecting [example-config.json](example-config.json) and replace 'YOUR BOT TOKEN HERE' with your token

4. Run Craig from within your output directory

   ```sh
   dotnet CraigBot.Bot.dll
   ```

### Hosting Craig

There are a number of ways you can host Craig permanently. Personally, I host Craig on a Raspberry Pi 4, which performs great within a small private guild. For this I use [screen](https://linuxize.com/post/how-to-use-linux-screen/) which allows me to keep Craig running without being SSH'd into the Pi. It's as simple as running the command below and then detaching from that screen session with `Ctrl+A D`.

```sh
screen dotnet CraigBot.Bot.dll
```

## Configuration

Craig can be easily configured to your liking by using your `config.json` file. See [example-config.json](example-config.json) or below for an example and brief explanation of the various settings.

```json
{
  "Bot": {
    "Token": "YOUR BOT TOKEN HERE",
    "Prefix": "!",
    "DmHelp": false,
    "MessageReward": 10,
    "StartingBalance": 10000,
    "Currency": "£",
    "MarketUpdateRate": 30
  },
  "ModuleFlags": {
    "Banking": true,
    "Betting": true,
    "Fun": true,
    "Help": true,
    "Image": true,
    "Miscellaneous": true,
    "Moderation": true,
    "Poll": true,
    "Utility": true
  }
}
```

### Bot

- `Token` - Your Discord bot's token, this is a requirement for your bot to startup its client
- `Prefix` - This will decided what symbol will be used to prefix your commands, i.e. `!help`, `-help`, `*help`, etc...
- `DmHelp` - Setting this to `true` will send the `!help` command list response to the requester as a DM as opposed to a message in the channel
- `MessageReward` - The amount of points/funds rewarded for posting a message, setting it to anything less than `0.01` will result in no rewards
- `StartingBalance` - The starting point/fund balance for new or existing users, setting it to anything less than `0.01` will result in a `0` starting balance
- `Currency` - The currency symbol you wish to prefix points/funds with, i.e. `£10.00`, `P10.00`, `$10.00`, etc...
- `MarketUpdateRate` - The rate (minutes) at which the stock market prices are updated

### ModuleFlags

You can easily disable modules by setting these to `false`. When disabled, the commands associated with these modules will not be accessible to anyone in guild regardless of roles/ownership. You need at least one module enabled to launch Craig.

## Built With

- [.NET Core](https://dotnet.microsoft.com/)
- [Discord.Net](https://github.com/RogueException/Discord.Net)
- [Entity Framework Core](https://github.com/aspnet/EntityFrameworkCore)
- [SQLite](https://www.sqlite.org/index.html)
- [Json.NET](https://github.com/JamesNK/Newtonsoft.Json)

## License

Distributed under the MIT License. Click [HERE](LICENSE) to view Craig's license.
