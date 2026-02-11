# Biphase-to-RSHW
A program that converts from the raw Biphase Mark WMA Show Files to an RSHW file that can be used in SPTE/RR-Engine.

## Why?
I spent quite a long time in the Rock-afire Explosion and CEC communities, and for a good chunk of that, a rumor flew around that there was this mystical program that could port the real show files into digital. I really didn't believe it, but one day I got a video of it in action as well as some first-hand accounts from people who used it. This was in roughly 2022.

After that, I departed from the RAE+CEC fandoms and instead went full-force on making programming and game development my career. But recently, I got the spark to revisit this project, as I had first attempted to make my own rendition as a wee novice and failed quite spectacularly. 

Somehow, in all the years I've stayed away from the fandom, this software is still a well-gatekept secret, and I've never truly been a fan of gatekeeping for no good reason.

So now, this is free for all forever. Call it giving back to the community after all this time.

## Usage
Download the most recent release, open a terminal pointing to the directory that contains the `BiphaseDecoder.exe` and run `BiphaseDecoder.exe input.wma output.rshw --verbose` or (`.\BiphaseDecoder.exe input.wma output.rshw --verbose` in Powershell), of course, subbing the file names for whatever you want.

## Known Issues
- It seems that .wma files outside of the scope of what ive tested simply dont work, im investigating this now.

## More?
Maybe! I work for a living, so small stuff like this really wont be my main priority, but I've got a patreon if any of you would like to support me. https://www.patreon.com/c/cometdev
