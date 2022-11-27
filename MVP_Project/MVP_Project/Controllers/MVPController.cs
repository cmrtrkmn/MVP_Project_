using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVP_Project.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MVP_Project.Models;
using System.Composition;
using System.Linq;

namespace MVP_Project.Controllers
{
    public class MVPController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }       

        [HttpPost("FileUpload")]
        public async Task<IActionResult> Index(List<IFormFile> files)
        {
            try
            {
                var size = files.Sum(f => f.Length);

                var filePaths = new List<string>();
                var players = new List<string>();
                List<BasketballPlayer> _playerListBasketball = new List<BasketballPlayer>();
                List<HandballPlayer> _playerListHandball = new List<HandballPlayer>();
                List<TotalPlayers> _totalPlayer = new List<TotalPlayers>();

                foreach (var formfile in files)
                {
                    if (formfile.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), formfile.FileName);
                        filePaths.Add(filePath);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formfile.CopyToAsync(stream);
                        }
                        if (filePath != null)
                        {
                            using (StreamReader file = new StreamReader(filePath))
                            {
                                int counter = 0;
                                string ln;
                                string match = "";
                                string[] matchDetail;
                                string matchName = "";
                                string player;
                                while ((ln = file.ReadLine()) != null)
                                {
                                    match += ln + "\n";
                                }
                                if (match != null)
                                {
                                    matchDetail = match.Split("\n");
                                    matchName = matchDetail[0].ToString();
                                    if(matchName.Trim()== "BASKETBALL")
                                    {
                                        for (int i = 1; i < matchDetail.Length - 1; i++)
                                        {
                                            //split for players
                                            player = matchDetail[i];
                                            string[] player_ = player.Split(";");

                                            BasketballPlayer _player = new BasketballPlayer();
                                            _player.PlayerName = player_[0].ToString();
                                            _player.NickName = player_[1].ToString();
                                            _player.Number = Convert.ToInt32(player_[2].ToString());
                                            _player.TeamName = player_[3].ToString();
                                            _player.position = player_[4].ToString();
                                            _player.ScoredPoint = Convert.ToInt32(player_[5].ToString());
                                            _player.Rebound = Convert.ToInt32(player_[6].ToString());
                                            _player.Assist = Convert.ToInt32(player_[7].ToString());

                                            _playerListBasketball.Add(_player);

                                        }
                                    }
                                    if(matchName.Trim() == "HANDBALL")
                                    {
                                        for (int i = 1; i < matchDetail.Length - 1; i++)
                                        {
                                            //split for players
                                            player = matchDetail[i];
                                            string[] player_ = player.Split(";");

                                            HandballPlayer _player = new HandballPlayer();
                                            _player.PlayerName = player_[0].ToString();
                                            _player.NickName = player_[1].ToString();
                                            _player.Number = Convert.ToInt32(player_[2].ToString());
                                            _player.TeamName = player_[3].ToString();
                                            _player.position = player_[4].ToString();
                                            if (_player.position.Trim() == "G") _player.InitialRatingPoints = 50;
                                            else if (_player.position.Trim() == "F") _player.InitialRatingPoints = 20;
                                            _player.GoalMade = Convert.ToInt32(player_[5].ToString());
                                            _player.GoalRecieved = Convert.ToInt32(player_[6].ToString());

                                            _playerListHandball.Add(_player);

                                        }

                                    }
                                    
                                }
                                //basketball rating point calculated
                                //
                                

                                if (_playerListBasketball.Count > 0)
                                {
                                    //rating points calculated
                                    foreach(var bas in _playerListBasketball)
                                    {
                                        //rules
                                        if(bas.position=="G")
                                        {
                                            bas.katSayiScoredPoint = 2;
                                            bas.katSayiRebound = 3;
                                            bas.katSayiAssist = 1;
                                        }
                                        if (bas.position == "F")
                                        {
                                            bas.katSayiScoredPoint = 2;
                                            bas.katSayiRebound = 2;
                                            bas.katSayiAssist = 2;
                                        }
                                        if (bas.position == "C")
                                        {
                                            bas.katSayiScoredPoint = 2;
                                            bas.katSayiRebound = 1;
                                            bas.katSayiAssist = 3;
                                        }
                                        
                                        bas.Rating_Points=bas.ScoredPoint*bas.katSayiScoredPoint+bas.Rebound*bas.katSayiRebound+bas.katSayiAssist *bas.katSayiAssist;
                                    }
                                    var winner = (from pl in _playerListBasketball
                                                 group pl by pl.TeamName into playerGroup
                                                 select new
                                                 {
                                                     team = playerGroup.Key,
                                                     scoredGroup = playerGroup.Sum(x => x.ScoredPoint)
                                                 }).ToList();
                                    if(winner[0].scoredGroup==winner[1].scoredGroup)
                                    {
                                        Response.WriteAsync("Must win a team!!");
                                        return Ok(); 
                                    }
                                    else
                                    {
                                       var _winner = winner.OrderByDescending(x => x.scoredGroup).First();
                                        var _winnerPlayers = _playerListBasketball.Where(x => x.TeamName == _winner.team).ToList();
                                        foreach(var _win in _winnerPlayers)
                                        {
                                            _win.Rating_Points += 10;
                                        }

                                        foreach(var pp in _playerListBasketball)
                                        {
                                            TotalPlayers ttPlayer = new TotalPlayers();
                                            ttPlayer.PlayerName = pp.PlayerName;
                                            ttPlayer.NickName = pp.NickName;
                                            ttPlayer.Number = pp.Number;
                                            ttPlayer.totalScored = pp.Rating_Points;
                                            _totalPlayer.Add(ttPlayer);
                                        }

                                    }
                                    _playerListBasketball.Clear();

                                }
                                

                                //handball rating point calculated
                                //
                                if (_playerListHandball.Count > 0)
                                {
                                    //rating points calculated
                                    foreach (var bas in _playerListHandball)
                                    {
                                        //rules
                                        if (bas.position == "G")
                                        {
                                            bas.factorGoalMade = 5;
                                            bas.factorGoalRecieved = -2;
                                        }
                                        if (bas.position == "F")
                                        {
                                            bas.factorGoalMade = 1;
                                            bas.factorGoalRecieved = -1;
                                        }


                                        bas.Rating_Points = bas.InitialRatingPoints + bas.GoalMade * bas.factorGoalMade + bas.GoalRecieved * bas.factorGoalRecieved;
                                    }
                                    var winner = (from pl in _playerListHandball
                                                  group pl by pl.TeamName into playerGroup
                                                  select new
                                                  {
                                                      team = playerGroup.Key,
                                                      goalMade = playerGroup.Sum(x => x.GoalMade)
                                                  }).ToList();
                                    if (winner[0].goalMade == winner[1].goalMade)
                                    {
                                        Response.WriteAsync("Must win a team!!");
                                        return Ok();
                                    }
                                    else
                                    {
                                        var _winner = winner.OrderByDescending(x => x.goalMade).First();
                                        var _winnerPlayers = _playerListHandball.Where(x => x.TeamName == _winner.team).ToList();
                                        foreach (var _win in _winnerPlayers)
                                        {
                                            _win.Rating_Points += 10;
                                        }

                                        foreach (var pp in _playerListHandball)
                                        {
                                            TotalPlayers ttPlayer = new TotalPlayers();
                                            ttPlayer.PlayerName = pp.PlayerName;
                                            ttPlayer.NickName = pp.NickName;
                                            ttPlayer.Number = pp.Number;
                                            ttPlayer.totalScored = pp.Rating_Points;
                                            _totalPlayer.Add(ttPlayer);
                                        }

                                    }
                                    _playerListHandball.Clear();
                                }


                                file.Close();
                            }
                        }


                    }
                    

                }
                var totalPlayer = (from pl in _totalPlayer
                                   group pl by new { pl.PlayerName, pl.NickName } into playerGroup
                                   select new
                                   {
                                       playerName = playerGroup.Key.PlayerName,
                                       nickName = playerGroup.Key.NickName,
                                       totalScored = playerGroup.Sum(x => x.totalScored)
                                   }).ToList();
                var _mvp = totalPlayer.OrderByDescending(x => x.totalScored).First();


                var message = "Most Valuable Player is: " +
                    "Player Name : " + _mvp.playerName + " " +
                    "Nick Name : " + _mvp.nickName + " " +
                    "Rating Points: " + _mvp.totalScored;
                return Ok(new { message });
            }
            catch
            {
                return (IActionResult)Response.WriteAsync("Files format is wrong. Dont calculate MVP");
            }
        }
    }
}
