using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using GTA;
using GTA.Native;
using GTA.Math;
using DiscordRPC;
using NativeUI;
using GTA.UI;
using System.Runtime.Remoting;

namespace DoraemonMenu
{
    public class Class1 : Script
    {
        MenuPool modMenuPool;
        UIMenu mainMenu;

        UIMenu playerMenu;
        UIMenu weaponsMenu;
        UIMenu vehicleMenu;
        UIMenu gameMenu;
        DiscordRpcClient client;

        UIMenuItem resetWantedLevel;

        public Class1()
        {
            client = new DiscordRpcClient("1131678664766009374");

            client.Initialize();

            client.SetPresence(new RichPresence()
            {
                Details = "Doraemon Menu for GTA",
                State = "SP Menu",
                Assets = new Assets()
                {
                    LargeImageKey = "doraemon",
                    LargeImageText = "Doraemon Menu",
                    SmallImageKey = "gta",
                    SmallImageText = "yes yes"
                },
                Buttons = new DiscordRPC.Button[]
                {
                    new DiscordRPC.Button() { Label = "Download", Url = "https://github.com/NotTacosdev/DoraemonMenuSP" }
                }
            });

            Setup();

            Tick += onTick;
            KeyDown += onKeyDown;
        }

        void Setup()
        {
            modMenuPool = new MenuPool();
            mainMenu = new UIMenu("Doraemon Menu", "Please select an option");
            modMenuPool.Add(mainMenu);

            playerMenu = modMenuPool.AddSubMenu(mainMenu, "Player");
            weaponsMenu = modMenuPool.AddSubMenu(mainMenu, "Weapons");
            vehicleMenu = modMenuPool.AddSubMenu(mainMenu, "Vehicles");
            gameMenu = modMenuPool.AddSubMenu(mainMenu, "GTA");

            SetupWeaponFunctions();
            SetupPlayerFunctions();
            SetupVehicleFunctions();
            SetupGameFunction();
        }

        void SetupGameFunction()
        {
            Savegame();
            radar();
            bark();
            scuba();
        }

        void Savegame()
        {
            UIMenuItem savegame = new UIMenuItem("Auto save game");
            gameMenu.AddItem(savegame);

            gameMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == savegame)
                {
                    Game.DoAutoSave(); // this should work
                    GTA.UI.Screen.ShowSubtitle("Automatically saved game!");
                }
            };
        }


        void SetupVehicleFunctions()
        {
            VehicleSelectorMenu();
            VehicleSpawnByName();
            RepairVehicle();
            bike();
            car();
            bypass();
            seatbelt();
            vehgodmode();
            vehautoclean();
        }

        void VehicleSelectorMenu()
        {
            UIMenu submenu = modMenuPool.AddSubMenu(vehicleMenu, "Vehicle Selector");

            List<dynamic> listOfVehicles = new List<dynamic>();
            VehicleHash[] allVehicleHashes = (VehicleHash[])Enum.GetValues(typeof(VehicleHash));
            for (int i = 0; i < allVehicleHashes.Length; i++)
            {
                listOfVehicles.Add(allVehicleHashes[i]);
            }

            UIMenuListItem list = new UIMenuListItem("Vehicle: ", listOfVehicles, 0);
            submenu.AddItem(list);

            UIMenuItem getVehicle = new UIMenuItem("Get Vehicle");
            submenu.AddItem(getVehicle);

            submenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == getVehicle)
                {
                    int listIndex = list.Index;
                    VehicleHash hash = allVehicleHashes[listIndex];

                    Ped gamePed = Game.Player.Character;

                    Vehicle v = World.CreateVehicle(hash, gamePed.Position, gamePed.Heading);
                    v.PlaceOnGround();
                    gamePed.Task.WarpIntoVehicle(v, VehicleSeat.Driver);
                }
            };
        }

        void tts()
        {
            UIMenu submenu = modMenuPool.AddSubMenu(playerMenu, "Voice Actor");

            UIMenuItem hi = new UIMenuItem("Hi");
            submenu.AddItem(hi);

            UIMenuItem no = new UIMenuItem("No");
            submenu.AddItem(no);

            UIMenuItem yes = new UIMenuItem("Yes");
            submenu.AddItem(yes);

            UIMenuItem howsitgoing = new UIMenuItem("How's it going");
            submenu.AddItem(howsitgoing);

            UIMenuItem insult = new UIMenuItem("Insult (High)");
            submenu.AddItem(insult);

            UIMenuItem bye = new UIMenuItem("Bye");
            submenu.AddItem(bye);

            UIMenuItem thanks = new UIMenuItem("Thanks");
            submenu.AddItem(thanks);

            UIMenuItem whatever = new UIMenuItem("Whatever");
            submenu.AddItem(whatever);

            UIMenuItem carcrash = new UIMenuItem("Car Crash");
            submenu.AddItem(carcrash);

            UIMenuItem kifflom = new UIMenuItem("Kifflom");
            submenu.AddItem(kifflom);

            UIMenuItem nicecar = new UIMenuItem("Nice Car");
            submenu.AddItem(nicecar);

            UIMenuItem reloading = new UIMenuItem("Reloading");
            submenu.AddItem(reloading);

            submenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == hi)
                {
                    Game.Player.Character.PlayAmbientSpeech("GENERIC_HI");
                }
                if (item == no)
                {
                    Game.Player.Character.PlayAmbientSpeech("GENERIC_NO");
                }
                if (item == yes)
                {
                    Game.Player.Character.PlayAmbientSpeech("GENERIC_YES");
                }
                if (item == howsitgoing)
                {
                    Game.Player.Character.PlayAmbientSpeech("GENERIC_HOWS_IT_GOING");
                }
                if (item == insult)
                {
                    Game.Player.Character.PlayAmbientSpeech("GENERIC_INSULT_HIGH");
                }
                if (item == bye)
                {
                    Game.Player.Character.PlayAmbientSpeech("GENERIC_BYE");
                }
                if (item == thanks)
                {
                    Game.Player.Character.PlayAmbientSpeech("GENERIC_THANKS");
                }
                if (item == whatever)
                {
                    Game.Player.Character.PlayAmbientSpeech("GENERIC_WHATEVER");
                }
                if (item == carcrash)
                {
                    Game.Player.Character.PlayAmbientSpeech("CRASH_CAR");
                }
                if (item == kifflom)
                {
                    Game.Player.Character.PlayAmbientSpeech("KIFFLOM_GREET");
                }
                if (item == nicecar)
                {
                    Game.Player.Character.PlayAmbientSpeech("NICE_CAR");
                }
                if (item == reloading)
                {
                    Game.Player.Character.PlayAmbientSpeech("RELOADING");
                }
            };
        }

        void VehicleSpawnByName()
        {
            UIMenuItem vehicleSpawnItem = new UIMenuItem("Spawn Vehicle By Name");
            vehicleMenu.AddItem(vehicleSpawnItem);
            vehicleMenu.OnItemSelect += (sender, item, index) =>
            {
                if(item == vehicleSpawnItem)
                {
                    string modelName = Game.GetUserInput("");
                    Model model = new Model(modelName);
                    model.Request();

                    Ped gamePed = Game.Player.Character;

                    if (model.IsInCdImage && model.IsValid)
                    {
                        Vehicle v = World.CreateVehicle(model, gamePed.Position, gamePed.Heading);
                        v.PlaceOnGround();
                        gamePed.Task.WarpIntoVehicle(v, VehicleSeat.Driver);
                    }
                }
            };
        }

        void RepairVehicle()
        {
            UIMenuItem vehicleRepairItem = new UIMenuItem("Repair Vehicle");
            vehicleMenu.AddItem(vehicleRepairItem);
            vehicleMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == vehicleRepairItem)
                {

                    if (Game.Player.Character.IsInVehicle())
                    {
                        Vehicle currentVehicle = Game.Player.Character.CurrentVehicle;

                        currentVehicle.Repair();
                        currentVehicle.Wash();

                        GTA.UI.Screen.ShowSubtitle("Car washed and repaired!");
                    }
                    else
                    {
                        GTA.UI.Screen.ShowSubtitle("You are not in a vehicle!");
                    }
                }
            };
        }

        void SetupPlayerFunctions()
        {
            resetWantedLevel = new UIMenuItem("Reset Wanted Level");
            playerMenu.AddItem(resetWantedLevel);

            playerMenu.OnItemSelect += onMainMenuItemSelect;

            godmode();
            godmodealt();
            neverwantedlevel();
            superjump();
            FixPlayer();
            superrun();
            superswim();
            refillsa();
            suicide();
            night();
            thermal();
            IgnoreP();
            tts();
            clearblood();
            noragdoll();
            maxwanted();
            tinyp();
            wet();
            money();
            removearmor();
            cloneped();
            abi();
            gravity();
            inv();
            less();
            autoclean();
            respawn();
        }

        void SetupWeaponFunctions()
        {
            WeaponSelectorMenu();
            GetAllWeapons();
            RemoveAllWeapons();
            infiniteammo();
            explosive();
            explosivemelee();
            flameammo();
        }

        bool ignorepon = false;
        void IgnoreP() // wtf am I doing 
        {
            UIMenuItem ignorep = new UIMenuItem("Cops don't shoot: " + ignorepon.ToString());
            playerMenu.AddItem(ignorep);


            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == ignorep)
                {
                    ignorepon = !ignorepon;

                    if (ignorepon)
                    {
                        Game.Player.IgnoredByPolice = true;
                        ignorep.Text = "Cops don't shoot: " + true.ToString();
                    }
                    else
                    {
                        Game.Player.IgnoredByPolice = false;
                        ignorep.Text = "Cops don't shoot: " + false.ToString();
                    }
                }
            };
        }

        bool invon = false;
        void inv() // wtf am I doing 
        {
            UIMenuItem inv = new UIMenuItem("Invisible: " + invon.ToString());
            playerMenu.AddItem(inv);


            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == inv)
                {
                    invon = !invon;

                    if (invon)
                    {
                        Game.Player.Character.IsVisible = false;
                        inv.Text = "Invisible: " + true.ToString();
                    }
                    else
                    {
                        Game.Player.Character.IsVisible = true;
                        inv.Text = "Invisible: " + false.ToString();
                    }
                }
            };
        }

        bool seatbelton = false;
        void seatbelt() // wtf am I doing 
        {
            UIMenuItem seatbelt = new UIMenuItem("Seatbelt: " + seatbelton.ToString());
            vehicleMenu.AddItem(seatbelt);


            vehicleMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == seatbelt)
                {
                    seatbelton = !seatbelton;

                    if (seatbelton)
                    {
                        Game.Player.Character.SetConfigFlag(32, false);
                        Game.Player.Character.CanBeKnockedOffBike = false;
                        seatbelt.Text = "Seatbelt: " + true.ToString();
                    }
                    else
                    {
                        Game.Player.Character.SetConfigFlag(32, true);
                        Game.Player.Character.CanBeKnockedOffBike = true;
                        seatbelt.Text = "Seatbelt: " + false.ToString();
                    }
                }
            };
        }

        bool lesson = false;
        void less() // why would you want this if you have god mode?
        {
            UIMenuItem less = new UIMenuItem("Take Less Damage: " + lesson.ToString());
            playerMenu.AddItem(less);


            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == less)
                {
                    lesson = !lesson;

                    if (lesson)
                    {
                        Game.Player.Character.CanSufferCriticalHits = false;
                        less.Text = "Take Less Damage: " + true.ToString();
                    }
                    else
                    {
                        Game.Player.Character.CanSufferCriticalHits = true;
                        less.Text = "Take Less Damage: " + false.ToString();
                    }
                }
            };
        }

        void wet() // wtf am I doing 
        {
            UIMenuItem wet = new UIMenuItem("Make Ped Wet");
            playerMenu.AddItem(wet);


            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == wet)
                {
                        Game.Player.Character.WetnessHeight = 1.99f;
                }
            };
        }

        void cloneped() // yes yes
        {
            UIMenuItem clone = new UIMenuItem("Clone Ped");
            playerMenu.AddItem(clone);


            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == clone)
                {
                    Game.Player.Character.Clone();
                }
            };
        }

        void money() // wheres mr krabs 
        {
            UIMenuItem money = new UIMenuItem("Get Maximum Money");
            playerMenu.AddItem(money);


            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == money)
                {
                    Game.Player.Money = 2147483647;
                    Game.Player.Character.Money = 2147483647; 
                }
            };
        }

        void removearmor() // idk why you want this but it's fine
        {
            UIMenuItem removearmor = new UIMenuItem("Remove Armor");
            playerMenu.AddItem(removearmor);


            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == removearmor)
                {
                    Game.Player.Character.Armor = 0;
                    GTA.UI.Screen.ShowSubtitle("Removed Armor!");
                }
            };
        }

        bool bikeon = false;
        void bike() // wtf am I doing 
        {
            UIMenuItem bike = new UIMenuItem("Can't be knocked off bike: " + bikeon.ToString());
            vehicleMenu.AddItem(bike);


            vehicleMenu.OnItemSelect += (sender, item, index) =>
            {
                bikeon = !bikeon;

                if (bikeon)
                {
                    Game.Player.Character.CanBeKnockedOffBike = false;
                    bike.Text = "Can't be knocked off bike: " + true.ToString();
                }
                else
                {
                    Game.Player.Character.CanBeKnockedOffBike = true;
                    bike.Text = "Can't be knocked off bike: " + false.ToString();
                }
            };
        }

        bool radaron = false;
        void radar() // sus
        {
            UIMenuItem radar = new UIMenuItem("Hide Radar: " + radaron.ToString());
            gameMenu.AddItem(radar);


            gameMenu.OnItemSelect += (sender, item, index) =>
            {
                radaron = !radaron;

                if (radaron)
                {
                    GTA.UI.Hud.IsRadarVisible = false;
                    radar.Text = "Hide Radar: " + true.ToString();
                }
                else
                {
                    GTA.UI.Hud.IsRadarVisible = true;
                    radar.Text = "Hide Radar: " + false.ToString();
                }
            };
        }

        bool caron = false;
        void car() // useless feature but great for menu 
        {
            UIMenuItem car = new UIMenuItem("Can't be dragged off vehicle: " + caron.ToString());
            vehicleMenu.AddItem(car);


            vehicleMenu.OnItemSelect += (sender, item, index) =>
            {
                caron = !caron;

                if (caron)
                {
                    Game.Player.Character.CanBeDraggedOutOfVehicle = false;
                    car.Text = "Can't be dragged off vehicle: " + true.ToString();
                }
                else
                {
                    Game.Player.Character.CanBeDraggedOutOfVehicle = true; // yes yes
                    car.Text = "Can't be dragged off vehicle: " + false.ToString();
                }
            };
        }

        bool tinypon = false;
        void tinyp() // wtf am I doing 
        {
            UIMenuItem tinyp = new UIMenuItem("Tiny Ped: " + tinypon.ToString());
            playerMenu.AddItem(tinyp);


            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == tinyp)
                {
                    tinypon = !tinypon;

                    if (tinypon)
                    {
                        Game.Player.Character.SetConfigFlag(223, true); // if this breaks, I'll switch it anyways
                        tinyp.Text = "Tiny Ped: " + true.ToString();
                    }
                    else
                    {
                        Game.Player.Character.SetConfigFlag(223, false);
                        tinyp.Text = "Tiny Ped: " + false.ToString();
                    }
                }
            };
        }

        bool autocleanon = false;
        void autoclean() // wtf am I doing 
        {
            UIMenuItem autoclean = new UIMenuItem("Auto Clean: " + autocleanon.ToString());
            playerMenu.AddItem(autoclean);


            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == autoclean)
                {
                    autocleanon = !autocleanon;

                    if (autocleanon)
                    {
                        autoclean.Text = "Auto Clean: " + true.ToString();
                    }
                    else
                    {
                        autoclean.Text = "Auto Clean: " + false.ToString();
                    }
                }
            };
        }

        void WeaponSelectorMenu()
        {
            UIMenu submenu = modMenuPool.AddSubMenu(weaponsMenu, "Weapon Selector Menu");

            List<dynamic> listOfWeapons = new List<dynamic>();
            WeaponHash[] allWeaponHashes = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
            for (int i = 0; i < allWeaponHashes.Length; i++)
            {
                listOfWeapons.Add(allWeaponHashes[i]);
            }

            UIMenuListItem list = new UIMenuListItem("Weapons: ", listOfWeapons, 0);
            submenu.AddItem(list);

            UIMenuItem getWeapon = new UIMenuItem("Get Weapon");
            submenu.AddItem(getWeapon);

            submenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == getWeapon)
                {
                    int listIndex = list.Index;
                    WeaponHash currentHash = allWeaponHashes[listIndex];
                    Game.Player.Character.Weapons.Give(currentHash, 9999, true, true);
                }
            };
        }

        bool noragdollon = false;
        void noragdoll()
        {
            UIMenuItem noragdoll = new UIMenuItem("No Ragdoll: " + noragdollon.ToString());
            playerMenu.AddItem(noragdoll);

            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == noragdoll)
                {
                    noragdollon = !noragdollon;

                    if (noragdollon)
                    {
                        Game.Player.Character.CanRagdoll = false;
                        noragdoll.Text = "No Ragdoll: " + true.ToString();
                    }
                    else
                    {
                        Game.Player.Character.CanRagdoll = true;
                        noragdoll.Text = "No Ragdoll: " + false.ToString();
                    }
                }
            };
        }

        bool bypasson = false;
        void bypass()
        {
            UIMenuItem bypass = new UIMenuItem("Bypass Max Vehicle Speed: " + bypasson.ToString());
            vehicleMenu.AddItem(bypass);

            vehicleMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == bypass)
                {
                    bypasson = !bypasson;

                    if (bypasson)
                    {
                        bypass.Text = "Bypass Max Vehicle Speed: " + true.ToString();
                    }
                    else
                    {
                        bypass.Text = "Bypass Max Vehicle Speed: " + false.ToString();
                    }
                }
            };
        }

        bool gravityon = false;
        void gravity()
        {
            UIMenuItem gravity = new UIMenuItem("No Gravity: " + gravityon.ToString());
            playerMenu.AddItem(gravity);

            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == gravity)
                {
                    gravityon = !gravityon;

                    if (gravityon)
                    {
                        gravity.Text = "No Gravity: " + true.ToString();
                    }
                    else
                    {
                        gravity.Text = "No Gravity: " + false.ToString();
                    }
                }
            };
        }

        bool abion = false;
        void abi()
        {
            UIMenuItem abi = new UIMenuItem("Unlimited Special Ability: " + abion.ToString());
            playerMenu.AddItem(abi);

            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == abi)
                {
                    abion = !abion;

                    if (abion)
                    {
                        abi.Text = "Unlimited Special Ability: " + true.ToString();
                    }
                    else
                    {
                        abi.Text = "Unlimited Special Ability: " + false.ToString();
                    }
                }
            };
        }

        void GetAllWeapons()
        {
            UIMenuItem allweapons = new UIMenuItem("Get all weapons");
            weaponsMenu.AddItem(allweapons);

            weaponsMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == allweapons)
                {
                    WeaponHash[] allweaponsHashes = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
                    for (int i = 0; i < allweaponsHashes.Length; i++)
                    {
                        Game.Player.Character.Weapons.Give(allweaponsHashes[i], 9999, true, true);
                    }
                }
            };
        }

        void FixPlayer()
        {
            UIMenuItem fixplayer = new UIMenuItem("Refill health and armor");
            playerMenu.AddItem(fixplayer);

            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == fixplayer)
                {
                    Game.Player.Character.Health = Game.Player.Character.MaxHealth;
                    Game.Player.Character.Armor = Game.Player.MaxArmor;
                    GTA.UI.Screen.ShowSubtitle("Health and armor restored!");
                }
            };
        }

        void clearblood()
        {
            UIMenuItem clearblood = new UIMenuItem("Clear Blood");
            playerMenu.AddItem(clearblood);

            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == clearblood)
                {
                    Game.Player.Character.ClearBloodDamage();
                    GTA.UI.Screen.ShowSubtitle("Blood Cleared!");
                }
            };
        }

   

        void refillsa()
        {
            UIMenuItem refillsa = new UIMenuItem("Refill Special Ability");
            playerMenu.AddItem(refillsa);

            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == refillsa)
                {
                    Game.Player.RefillSpecialAbility();
                    GTA.UI.Screen.ShowSubtitle("Special Ability restored!");
                }
            };
        }

        void suicide()
        {
            UIMenuItem suicide = new UIMenuItem("Suicide");
            playerMenu.AddItem(suicide);

            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == suicide)
                {
                    Game.Player.Character.Kill();
                }
            };
        }

        bool thermalon = false;
        void thermal()
        {
            UIMenuItem thermal = new UIMenuItem("Thermal Vision: " + thermalon.ToString());
            playerMenu.AddItem(thermal);


            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == thermal)
                {
                    thermalon = !thermalon;

                    if (thermalon)
                    {
                        Game.IsThermalVisionActive = true;
                        thermal.Text = "Thermal Vision: " + true.ToString();
                    }
                    else
                    {
                        Game.IsThermalVisionActive = false;
                        thermal.Text = "Thermal Vision: " + false.ToString();
                    }
                }
            };
        }

        bool nighton = false;
        void night()
        {
            UIMenuItem night = new UIMenuItem("Night Vision: " + nighton.ToString());
            playerMenu.AddItem(night);


            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == night)
                {
                    nighton = !nighton;

                    if (nighton)
                    {
                        Game.IsNightVisionActive = true;
                        night.Text = "Night Vision: " + true.ToString();
                    }
                    else
                    {
                        Game.IsNightVisionActive = false;
                        night.Text = "Night Vision: " + false.ToString();
                    }
                }
            };
        }


        bool infiniteammoon = false;
        void infiniteammo()
        {
            UIMenuItem infiniteammo = new UIMenuItem("Infinite ammo: " + infiniteammoon.ToString());
            weaponsMenu.AddItem(infiniteammo);


            weaponsMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == infiniteammo)
                {
                    infiniteammoon = !infiniteammoon;

                    if (infiniteammoon)
                    {
                        Weapon w = Game.Player.Character.Weapons.Current;
                        w.InfiniteAmmoClip = true;

                        infiniteammo.Text = "Infinite ammo: " + true.ToString();
                    }
                    else
                    {
                        Weapon w = Game.Player.Character.Weapons.Current;
                        w.InfiniteAmmoClip = false;

                        infiniteammo.Text = "Infinite ammo: " + false.ToString();
                    }
                }
            };
        }

        bool flameammoon = false;
        void flameammo()
        {
            UIMenuItem flameammo = new UIMenuItem("Flame ammo: " + flameammoon.ToString());
            weaponsMenu.AddItem(flameammo);


            weaponsMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == flameammo)
                {
                    flameammoon = !flameammoon;

                    if (flameammoon)
                    {
                        flameammo.Text = "Flame ammo: " + true.ToString();
                    }
                    else
                    {
                        flameammo.Text = "Flame ammo: " + false.ToString();
                    }
                }
            };
        }

        void RemoveAllWeapons()
        {
            UIMenuItem removeButton = new UIMenuItem("Remove All Weapons");

            WeaponHash[] allWeaponHashes = (WeaponHash[])Enum.GetValues(typeof(WeaponHash));
            weaponsMenu.AddItem(removeButton);
            weaponsMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == removeButton)
                {
                    foreach(WeaponHash hash in allWeaponHashes)
                    {
                        if (hash != WeaponHash.Unarmed)
                        {
                            if (Game.Player.Character.Weapons.HasWeapon(hash))
                                Game.Player.Character.Weapons.Remove(hash);
                        }
                    }
                }
            };
        }

        bool explosiveon = false;
        void explosive()
        {
            UIMenuItem explosiveammo = new UIMenuItem("Explosive Ammo: " + explosiveon.ToString());

            weaponsMenu.AddItem(explosiveammo);

            weaponsMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == explosiveammo)
                {
                    explosiveon = !explosiveon;

                    if (explosiveon)
                        explosiveammo.Text = "Explosive Ammo: " + true.ToString();
                    else
                        explosiveammo.Text = "Explosive Ammo: " + false.ToString();

                }
            };
        }

        bool explosivemeleeon = false;
        void explosivemelee()
        {
            UIMenuItem explosivemelee = new UIMenuItem("Explosive Melee: " + explosivemeleeon.ToString());

            weaponsMenu.AddItem(explosivemelee);

            weaponsMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == explosivemelee)
                {
                    explosivemeleeon = !explosivemeleeon;

                    if (explosivemeleeon)
                        explosivemelee.Text = "Explosive Melee: " + true.ToString();
                    else
                        explosivemelee.Text = "Explosive Melee: " + false.ToString();

                }
            };
        }

        bool barkon = false;
        void bark()
        {
            UIMenuItem bark = new UIMenuItem("Disable Dog Barking: " + barkon.ToString());

            gameMenu.AddItem(bark);

            gameMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == bark)
                {
                    barkon = !barkon;

                    if (barkon)
                    {
                        Audio.SetAudioFlag(AudioFlags.DisableBarks, true);
                        bark.Text = "Disable Dog Barking: " + true.ToString(); //brackets is the way for this to be fixed
                    }
                    else
                    {
                        Audio.SetAudioFlag(AudioFlags.DisableBarks, false);
                        bark.Text = "Disable Dog Barking: " + false.ToString();
                    }

                }
            };
        }

        bool scubaon = false;
        void scuba()
        {
            UIMenuItem scuba = new UIMenuItem("Scuba Breathing Audio: " + scubaon.ToString());

            gameMenu.AddItem(scuba);

            gameMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == scuba)
                {
                    scubaon = !scubaon;

                    if (scubaon)
                    {
                        Audio.SetAudioFlag(AudioFlags.SuppressPlayerScubaBreathing, true);
                        scuba.Text = "Scuba Breathing Audio: " + true.ToString(); //brackets is the way for this to be fixed
                    }
                    else
                    {
                        Audio.SetAudioFlag(AudioFlags.SuppressPlayerScubaBreathing, false);
                        scuba.Text = "Scuba Breathing Audio: " + false.ToString();
                    }

                }
            };
        }

        void maxwanted() // this code will prob break anyways, but hey, atleast I tried!
        {

            UIMenuItem maxwanted = new UIMenuItem("Max Wanted Level");
            playerMenu.AddItem(maxwanted);

            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == maxwanted)
                {
                    if (Game.Player.WantedLevel == 5)
                    {
                        GTA.UI.Screen.ShowSubtitle("Your wanted level is already full!");
                    }
                    else
                    {
                        Game.Player.WantedLevel = 5;
                    }
                }
            };
        }

        void onMainMenuItemSelect(UIMenu sender, UIMenuItem item, int index)
        {
            if (item == resetWantedLevel)
            {
                if (Game.Player.WantedLevel == 0)
                {
                    GTA.UI.Screen.ShowSubtitle("You have no wanted levels!");
                }
                else
                {
                    Game.Player.WantedLevel = 0;
                }
            }
        }

        bool godmodeon = false;
        void godmode()
        {
            UIMenuItem godmode = new UIMenuItem("God Mode: " + godmodeon.ToString());

            playerMenu.AddItem(godmode);

            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == godmode)
                {
                    godmodeon = !godmodeon;

                    if (godmodeon)
                    {
                        Game.Player.IsInvincible = true;
                        Game.Player.Character.IsInvincible = true;

                        godmode.Text = "God Mode: " + true.ToString();
                    }
                    else
                    {
                        Game.Player.IsInvincible = false;
                        Game.Player.Character.IsInvincible = false;

                        godmode.Text = "God Mode: " + false.ToString();
                    }
                }
            };
        }

        bool godmodealton = false;
        void godmodealt()
        {
            UIMenuItem godmodealt = new UIMenuItem("God Mode V2: " + godmodealton.ToString()); // trying something new

            playerMenu.AddItem(godmodealt);

            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == godmodealt)
                {
                    godmodealton = !godmodealton;

                    if (godmodealton)
                    {
                        Game.Player.IsInvincible = true;
                        Game.Player.Character.IsInvincible = true;

                        godmodealt.Text = "God Mode V2: " + true.ToString();
                    }
                    else
                    {
                        Game.Player.IsInvincible = false;
                        Game.Player.Character.IsInvincible = false;

                        godmodealt.Text = "God Mode V2: " + false.ToString();
                    }
                }
            };
        }

        bool vehgodmodeon = false;
        void vehgodmode()
        {
            UIMenuItem vehgodmode = new UIMenuItem("Vehicle God Mode: " + vehgodmodeon.ToString());

            vehicleMenu.AddItem(vehgodmode);

            vehicleMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == vehgodmode)
                {
                    vehgodmodeon = !vehgodmodeon;

                    if (vehgodmodeon)
                    {
                        Game.Player.Character.CurrentVehicle.IsInvincible = true;

                        vehgodmode.Text = "God Mode: " + true.ToString();
                    }
                    else
                    {
                        Game.Player.Character.CurrentVehicle.IsInvincible = false;

                        vehgodmode.Text = "God Mode: " + false.ToString();
                    }
                }
            };
        }

        bool superrunon = false;
        void superrun()
        {
            UIMenuItem superrun = new UIMenuItem("Super Run: " + superrunon.ToString());

            playerMenu.AddItem(superrun);

            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == superrun)
                {
                    superrunon = !superrunon;

                    if (superrunon)
                    {
                        superrun.Text = "Super Run: " + true.ToString();
                    }
                    else
                    {
                        superrun.Text = "Super Run: " + false.ToString();
                    }
                }
            };
        }

        bool neverwantedlevelon = false;
        void neverwantedlevel()
        {
            UIMenuItem neverWanted = new UIMenuItem("Never Wanted: " + neverwantedlevelon.ToString());

            playerMenu.AddItem(neverWanted);

            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == neverWanted)
                {
                    neverwantedlevelon = !neverwantedlevelon;

                    if (neverwantedlevelon)
                        neverWanted.Text = "Never Wanted: " + true.ToString();
                    else
                        neverWanted.Text = "Never Wanted: " + false.ToString();

                }
            };
        }

        bool respawnon = false;
        void respawn()
        {
            UIMenuItem respawn = new UIMenuItem("Fast Respawn: " + respawnon.ToString());

            playerMenu.AddItem(respawn);

            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == respawn)
                {
                    respawnon = !respawnon;

                    if (respawnon)
                        respawn.Text = "Fast Respawn: " + true.ToString();
                    else
                        respawn.Text = "Fast Respawn: " + false.ToString();

                }
            };
        }

        bool vehautocleanon = false;
        void vehautoclean()
        {
            UIMenuItem vehautoclean = new UIMenuItem("Vehicle Auto Clean: " + vehautocleanon.ToString());

            vehicleMenu.AddItem(vehautoclean);

            vehicleMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == vehautoclean)
                {
                    vehautocleanon = !vehautocleanon;

                    if (vehautocleanon)
                        vehautoclean.Text = "Vehicle Auto Clean: " + true.ToString();
                    else
                        vehautoclean.Text = "Vehicle Auto Clean: " + false.ToString();

                }
            };
        }

        bool superjumpon = false;
        void superjump()
        {
            UIMenuItem superJump = new UIMenuItem("Super Jump: " + superjumpon.ToString());

            playerMenu.AddItem(superJump);

            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == superJump)
                {
                    superjumpon = !superjumpon;

                    if (superjumpon)
                        superJump.Text = "Super Jump: " + true.ToString();
                    else
                        superJump.Text = "Super Jump: " + false.ToString();

                }
            };
        }

        bool superswimon = false;
        void superswim()
        {
            UIMenuItem superSwim = new UIMenuItem("Super Swim: " + superswimon.ToString());

            playerMenu.AddItem(superSwim);

            playerMenu.OnItemSelect += (sender, item, index) =>
            {
                if (item == superSwim)
                {
                    superswimon = !superswimon;

                    if (superswimon)
                        superSwim.Text = "Super Swim: " + true.ToString();
                    else
                        superSwim.Text = "Super Swim: " + false.ToString();
                }
            };
        }

        void onTick(object sender, EventArgs e)
        {
            if (modMenuPool != null)
                modMenuPool.ProcessMenus();

            if(neverwantedlevelon)
            {
                Game.Player.WantedLevel = 0;
            }
            if (superjumpon)
            {
                Game.Player.SetSuperJumpThisFrame();
            }
            if (explosiveon)
            {
                Game.Player.SetExplosiveAmmoThisFrame();
            }
            if (explosivemeleeon)
            {
                Game.Player.SetExplosiveMeleeThisFrame();
            }
            if (flameammoon)
            {
                Game.Player.SetFireAmmoThisFrame();
            }
            if (superrunon)
            {
                Game.Player.SetRunSpeedMultThisFrame((float)1.5);
            }
            if (superswimon)
            {
                Game.Player.SetSwimSpeedMultThisFrame((float)1.5);
            }
            if (abion)
            {
                Game.Player.RefillSpecialAbility();
            }
            if (gravityon)
            {
                Game.Player.Character.HasGravity = false;
            }
            if (bypasson)
            {
                Game.Player.Character.MaxDrivingSpeed = 999999999;
            }
            if (autocleanon)
            {
                Game.Player.Character.ClearBloodDamage();
                Game.Player.Character.ClearVisibleDamage();
            }
            if (vehautocleanon)
            {
                Game.Player.Character.CurrentVehicle.Wash();
            }
            if (respawnon)
            {
                if (Game.Player.Character.IsDead)
                {
                    Game.Player.Character.Resurrect();
                }
            }
            if (godmodealton)
            {
                Game.Player.Character.Armor = Game.Player.MaxArmor;
            }
        }

        void onKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F10 && !modMenuPool.IsAnyMenuOpen())
            {
                mainMenu.Visible = !mainMenu.Visible;
            }
        }
    }
}
