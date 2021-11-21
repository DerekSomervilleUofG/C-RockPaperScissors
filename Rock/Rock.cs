using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;


namespace Rock
{
    class Rock
    {
        private Config _config = new ConfigFromFile();

        private UserInput _userInput = new ConsoleInput();
        private UserOutput _userOutput = new ConsoleOutput();

        private string[] _property = new string[4];

        public void SetUserInput(UserInput userInput)
        {
            this._userInput = userInput;
        }
        public void SetUserOutput(UserOutput userOutput)
        {
            this._userOutput = userOutput;
        }

        public void SetConfig(Config config)
        {
            this._config = config;
        }

        private string generateRequest(String[] weapons)
        {
            String request = "Please select";
            int i = 0;
            foreach (string weapon in weapons)
            {
                i++;
                request += " " + i + ". " + weapon;
            }
            request += " 4. Exit";
            return request;
        }
        private int requestPlay(String[] weapons)
        {
            String request;
            request = generateRequest(weapons);
            this._userOutput.Output(request);

            int userWeapon = this._userInput.GetInputInt() - 1;

            return userWeapon;
        }
        private void displayWinner(string winner)
        {
            this._userOutput.Output(winner);
        }
        private string determineWinner(String[] weaponList, int userWeapon, int computerWeapon)
        {
            String winner;
            if (userWeapon == computerWeapon)
            {
                winner = "Draw both selected " + weaponList[computerWeapon];
            }
            else if ((userWeapon + 1) % weaponList.Length == computerWeapon)
            {
                winner = "You win and beat the computer's " + weaponList[computerWeapon];
            }
            else if ((computerWeapon + 1) % weaponList.Length == userWeapon)
            {
                winner = "Computer wins with " + weaponList[computerWeapon];
            }
            else
            {
                winner = "Please select 1. Rock, 2. Scissors or 3. Paper";
            }

            return winner;

        }

        private string[] getWeaponList()
        {
            string request = this.generateGamesListRequest();
            List<string[]> weaponLists = this.getWeaponLists(this._property);
            this._userOutput.Output(request);
            int userGame = this._userInput.GetInputInt();
            return weaponLists[userGame];
        }

        private void play()
        {
            string[] weaponList = getWeaponList();
            int userWeapon;
            userWeapon = this.requestPlay(weaponList);
            while (userWeapon < weaponList.Length)
            {
                int computerWeapon;
                string winner;
                Random random = new Random();
                computerWeapon = random.Next(0, weaponList.Length - 1);

                winner = this.determineWinner(weaponList, userWeapon, computerWeapon);
                this.displayWinner(winner);
                userWeapon = requestPlay(weaponList);
            }
        }

        public String generateGamesListRequest()
        {
            List<String> listOfGames = getListOfGames();
            String request = getGamesRequest(listOfGames);
            return request;
        }

        public List<string[]> getWeaponLists(string[] property)
        {
            List<string[]> weaponLists = new List<string[]>();
            if (this._property is null)
            {
                this._property = _config.GetConfig();
            }
            for (int counter = 1; counter < property.Count(); counter++)
            {
                weaponLists.Add(property[counter].Split(":")[1].Split(","));
            }
            return weaponLists;
        }

        public List<String> getListOfGames()
        {
            List<String> listOfGames = new List<String>();

            this._property = _config.GetConfig();

            for (int counter = 1; counter < this._property.Length; counter++)
            {
                listOfGames.Add(this._property[counter].Split(":")[0]);
            }
            return listOfGames;
        }


        private string getGamesRequest(List<string> listOfGames)
        {
            string request = "Please select";
            for (int counter = 0; counter < listOfGames.Count(); counter++)
            {
                request += " " + counter.ToString() + " - " + listOfGames[counter];
            }
            return request;
        }

        static void Main(string[] args)
        {
            var weaponList = new String[] { "Rock", "Scissors", "Paper" };
            Rock rock = new Rock();
            rock.play();
        }
    }
    interface Config
    {
        string[] GetConfig();
    }

    class ConfigFromFile : Config
    {
        public string[] GetConfig()
        {

            fixWorkingDirectory();
            string[] propertyData = null;

            string configPath = "resource/Config/properties.cfg";
            try 
            { 

                propertyData = File.ReadAllLines(configPath);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("getConfig");
                Console.WriteLine(e.Message);
                Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
            }

            return propertyData;
        }

        private void fixWorkingDirectory()
        {
            String currentDirectory = System.IO.Directory.GetCurrentDirectory();
            while (currentDirectory.Contains("bin"))
            {
                System.IO.Directory.SetCurrentDirectory(System.IO.Directory.GetParent(currentDirectory).FullName);
                currentDirectory = System.IO.Directory.GetCurrentDirectory();
            }
        }
    }

    class ConfigFromStubb : Config
    {

        public string[] GetConfig()
        {
            string[] propertyData = new string[4];
            propertyData.Append("Name:First,Second,Third");
            propertyData.Append("Rock Paper Scissors:Rock,Scissors,Paper");
            propertyData.Append("Star Wars:Darth Vadar,Emperor,Luke Skywalker");
            return propertyData;
        }
    }

    interface UserInput
    {
        string GetInputString();
        int GetInputInt();
    }

    class ConsoleInput : UserInput
    {
        public string GetInputString()
        {
            return Console.ReadLine();
        }
        public int GetInputInt()
        {
            return Convert.ToInt32(GetInputString());
        }
    }

    interface UserOutput
    {
        public void Output(string output);
    }

    class ConsoleOutput : UserOutput
    {
        public void Output(string output)
        {
            Console.WriteLine(output);
        }
    }
}
