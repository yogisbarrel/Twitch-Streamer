#
# getGames.ps1
#
function Get-TwitchGames
{
    [CmdletBinding()]
    Param(   
    [Parameter(Mandatory=$true, Position=0, ValueFromPipeline=$true)]
        [int]$Limit)
    [int]$i = 0
    $gameJson = Invoke-WebRequest -Uri "https://api.twitch.tv/kraken/games/top?limit=$Limit" -Method Get
    $gameConvert = ConvertFrom-Json -InputObject $gameJson.content
    $gameNames = $gameConvert.top.game.name

    $gameNames | ForEach-Object {
       $games = [ordered]@{

                "Name" = $null; #name of game
                "Preview" = $null; #game image for button
                }    
    $games["Name"] = $gameConvert.top.game.name[$i]
    $games["Preview"] = $gameConvert.top.game.box.medium[$i]
    Write-Output $games
    $i++
    }    
}