﻿using System;
using System.ComponentModel;
using System.Formats.Asn1;
using System.Runtime.CompilerServices;

namespace RPG_Dungeon_Crawler
{
    class Program
    {

        static public bool GameOver = false;
        static public int WinCount;
        static public int LoseCount;
        static public bool Reset = false;
        
        // Weapon class
        abstract class Weapon
        {
            public readonly int Power;
            public readonly string WeaponName;

            public Weapon(string name, int power)
            {
                Power = power;
                WeaponName = name;
            }
        }
        
        // Every weapon and their power
        class TrainingWeapon : Weapon
        {
            public TrainingWeapon() : base ("Training Weapon", 66) {}
        }

        class BoneSword : Weapon
        {
            public BoneSword() : base ("Bone Sword", 46) {}
        }

        class CursedStaff : Weapon
        {
            public CursedStaff() : base("Cursed Staff", 32) {}
        }

        class Unarmed : Weapon
        {
            public Unarmed() : base("Unarmed", 0) {}
        } 

        class Spike : Weapon
        {
            public Spike() : base("Spike", 27) {}
        }

        class Haunt : Weapon
        {
            public Haunt() : base("Haunt", 10) {}
        }
        
        class Charge : Weapon
        {
            public Charge() : base("Charge", 50) {}
        }

        // Unit class
        abstract class Unit
        {
            private readonly string name;

            public Unit(string name, int maxHealth, Weapon weapon)
            {
                if (GameOver == true)
                {
                    nextId = 0;
                }
                
                this.name = name;
                id = nextId;
                nextId++;
                health = maxHealth;
                this.maxHealth = maxHealth;
                Weapon = weapon;
                ReportStatus();
            }

            private int maxHealth;
            private int health;
            public int id;
            private static int nextId;
            private Weapon Weapon;

            


            // Attack method
            public virtual void Attack(Unit target)
            {
                Console.WriteLine($"Unit #{id}: {Name} uses {Weapon.WeaponName} to attack Unit #{target.id}: {target.Name} for {Weapon.Power} Damage.");
                target.TakeDamage(Weapon.Power);
            }

            // Life and death logic
            ~Unit()
            {
                if (GameOver != true)
                {
                    Console.WriteLine($"Unit #{id} {name} was vanquished!");
                }
            }

            public string Name => name;
            
            
            public bool IsAlive
            {
                get
                {
                    if (true)
                    {
                        return health > 0;
                    }

                    else
                    {
                        return false;
                    }
                }
            }

            public bool IsDead
            {
                get
                {
                    if (true)
                    {
                        return !IsAlive;
                    }

                    else
                    {
                        return false;
                    }
                }
            }
            
            // Health, MaxHealth & TakeDamage

            public int MaxHealth
            {
                get => maxHealth;
            }

            public int Health
            {
                set
                {
                    health = Math.Clamp(value, 0, maxHealth);
                    ReportStatus();
                }
                get => health;
            }
            

            public virtual void TakeDamage(int value)
            {
                Health -= value;
            }

            public void ReportStatus()
            {
                if (Reset == false)
                {
                    Console.WriteLine($"Unit #{id}: {name} - {health}/{maxHealth} Health");
                }
            }
        }
        
        // Hero Unit
        class Hero : Unit
        {
            public Hero() : base("Hero", 1000, new TrainingWeapon())
            {
                Console.WriteLine(@"
      _,.
    ,` -.)
   ( _/-\\-._
  /,|`--._,-^|            ,
  \_| |`-._/||          ,'|
    |  `-, / |         /  /
    |     || |        /  /
     `r-._||/   __   /  /
 __,-<_     )`-/  `./  /
'  \   `---'   \   /  /
    |           |./  /
    /           //  /
\_/' \         |/  /
 |    |   _,^-'/  /
 |    , ``  (\/  /_
  \,.->._    \X-=/^
  (  /   `-._//^`
   `Y-.____(__}
    |     {__)
          ()");
                Console.WriteLine();
                Console.WriteLine(@"You enter the dungeon clad in armour.
Though it is murky, you sense something waiting up ahead.
Alas, bursting from the shadows, something sinister springs towards you...");
                Console.ReadLine();
                Console.WriteLine();
            }

            public override void Attack(Unit target)
            {
                if (target is Skeleton)
                {
                    base.Attack(target);
                    Console.WriteLine($"The Hero deals 10 extra Damage against the {target.Name}'s weak Bones!\n");
                    target.TakeDamage(+10);
                    Console.WriteLine();
                }

                if (target is Ghost)
                {
                    int random2 = random.Next(0, 100);

                    if (random2 < 55)
                    {
                        Console.WriteLine("The Hero is too scared to attack!\n");
                    }
                    else
                    {
                        base.Attack(target);
                    }
                }
                
                else
                {
                    base.Attack(target);
                }
            }
        }
        
        
        // Skeleton Unit
        class Skeleton : Unit
        {
            public Skeleton() : base("Skeleton", 250, new BoneSword()) {}
        }

        class Ghost : Unit
        {
            public Ghost() : base("Ghost", 200, new Haunt()) {}
        }

        class Jackalope : Unit
        {
            public Jackalope() : base("Jackalope", 199, new Charge()) {}

            public override void Attack(Unit target)
            {
                int random3 = random.Next(0, 10);

                if (random3 < 3)
                {
                    Console.WriteLine("The fearsome Jackalope landed a critical hit!");
                    Console.ReadLine();
                    target.TakeDamage(+100);
                }

                else
                {
                    base.Attack(target);
                }
            }
        }


        // Bomb class 
        class Bomb : Unit
        {
            public Bomb() : base("Bomb", 500, new Unarmed())
            {
                BombRounds = 0;
                Explosion = false;
            }
            private int BombRounds;
            private bool Explosion = false;
            
            
            // Explosion logic
            public override void TakeDamage(int value)
            {
                if ((BombRounds < 5) && (IsAlive))
                {
                    BombRounds++;
                    base.TakeDamage(value);
                    if (this.IsAlive)
                    {
                        Console.WriteLine($"{Name} will explode in {(6-BombRounds)} rounds!");
                    }
                }
                else 
                {
                    Console.WriteLine("The bomb explodes!");
                    Explosion = true;
                }
                
            }
            // Explosion hurts Hero & destroys bomb
            public override void Attack(Unit target)
            {
                if (Explosion == true)
                {
                    Console.WriteLine("The explosion hurts the hero!\n");
                    Console.ReadLine();
                    target.TakeDamage(+500);
                    Health = 0;
                    Explosion = false;
                }
            }
        }
        
        // Hedgehog Unit
        class Hedgehog : Unit
        {
            public Hedgehog() : base("Hedgehog", 200,  new Spike())
            {
                isDefenseMode = false;
                DefenseModeRounds = 0;
            }
             
            // Defense mode
            public override void TakeDamage(int value)
            {
                if ((isDefenseMode == false) && (DefenseModeRounds == 0))
                {
                    isDefenseMode = true;
                    base.TakeDamage(value);
                }

                if (DefenseModeRounds == 2)
                {
                    isDefenseMode = false;
                    DefenseModeRounds = 0;
                    Console.WriteLine($"{Name} stopped being in Defense Mode!\n");
                }

                if ((DefenseModeRounds < 2) && (isDefenseMode) && (this.IsAlive))
                {
                    DefenseModeRounds++;
                    Console.WriteLine($"{Name} went into Defense Mode!\n");
                }

            }

            public int DefenseModeRounds;
            public bool isDefenseMode;
        }
        
        
        // necromancer unit & resurrection
        class Necromancer : Unit
        {
            bool hasResurrected;

            public Necromancer() : base("Necromancer", 200, new CursedStaff()) { }

            public override void TakeDamage(int value)
            {
                base.TakeDamage(value);
                Resurrect();
            }

            // resurrect logic
            private void Resurrect()
            {
               
                if ((IsDead) && (!hasResurrected))
                {
                    hasResurrected = true;
                    Health = MaxHealth / 2;
                    Console.WriteLine($@"Necromancer has resurrected itself!
                 /\
                 ||
   ____ (((+))) _||_
  /.--.\  .-.  /.||.\
 /.,   \\(0.0)// || \\
/;`';/\ \\|m|//  ||  ;\
|:   \ \__`:`____||__:|
|:    \__ \T/ (@~)(~@)|
|:    _/|     |\_\/  :|
|:   /  |     |  \   :|
|'  /   |     |   \  '|
 \_/    |     |    \_/
        |     |
        |_____|
        |_____|
");
                    Console.ReadLine();
                    ReportStatus();
                }
            }
        }

        
        // random logic
        private static readonly Random random = new Random();
        
        // Main Program
        static void Main(string[] args)
        {
            Console.WriteLine(@"
████████▄  ███    █▄  ███▄▄▄▄      ▄██████▄     ▄████████  ▄██████▄  ███▄▄▄▄   
███   ▀███ ███    ███ ███▀▀▀██▄   ███    ███   ███    ███ ███    ███ ███▀▀▀██▄
███    ███ ███    ███ ███   ███   ███    █▀    ███    █▀  ███    ███ ███   ███
███    ███ ███    ███ ███   ███  ▄███         ▄███▄▄▄     ███    ███ ███   ███
███    ███ ███    ███ ███   ███ ▀▀███ ████▄  ▀▀███▀▀▀     ███    ███ ███   ███
███    ███ ███    ███ ███   ███   ███    ███   ███    █▄  ███    ███ ███   ███
███   ▄███ ███    ███ ███   ███   ███    ███   ███    ███ ███    ███ ███   ███
████████▀  ████████▀   ▀█   █▀    ████████▀    ██████████  ▀██████▀   ▀█   █▀ 


 ▄████████    ▄████████    ▄████████  ▄█     █▄   ▄█          ▄████████    ▄████████
███    ███   ███    ███   ███    ███ ███     ███ ███         ███    ███   ███    ███
███    █▀    ███    ███   ███    ███ ███     ███ ███         ███    █▀    ███    ███
███         ▄███▄▄▄▄██▀   ███    ███ ███     ███ ███        ▄███▄▄▄      ▄███▄▄▄▄██▀
███        ▀▀███▀▀▀▀▀   ▀███████████ ███     ███ ███       ▀▀███▀▀▀     ▀▀███▀▀▀▀▀
███    █▄  ▀███████████   ███    ███ ███     ███ ███         ███    █▄  ▀███████████
███    ███   ███    ███   ███    ███ ███ ▄█▄ ███ ███▌    ▄   ███    ███   ███    ███
████████▀    ███    ███   ███    █▀   ▀███▀███▀  █████▄▄██   ██████████   ███    ███

 
                                                                               ");
            Console.ReadLine();
            Console.WriteLine("Fight your way through a dungeon of monsters to the end!");
            Console.WriteLine("Press 'Enter' when ready to play.\n");
            Console.ReadLine();
            
            bool playerWantsToQuit = false;
            
            // keep playing until player wants to quit
            while (playerWantsToQuit == false)
            {
                // new hero and replay logic
                Reset = false;
                Unit hero = new Hero();
                GameOver = false;

                while (hero.IsAlive == true)
                {
                    for (int i = 0; i < 3; i++)
                    {
                       //add new units
                        Unit unit = SpawnNewUnit();
                        while (unit.IsAlive == true && hero.IsAlive == true)
                        {
                            Console.WriteLine();
                            Console.WriteLine("The fight continues... (Press any key.)\n");
                            Console.ReadKey();
                            hero.Attack(unit);
                            if (unit.IsAlive)
                            {
                                Console.ReadLine();
                                Console.WriteLine();
                                unit.Attack(hero);
                            }
                        }

                        if (hero.IsAlive)
                        {
                            Console.WriteLine($"{unit.Name} was defeated!\n");
                            Console.ReadLine();
                        }
                    }

                    if (hero.IsDead == true)
                    {
                        LoseCount++;
                        Console.ReadLine();
                        Console.WriteLine("The hero died! You Lose.");
                        Console.WriteLine(@"

   ▄██████▄     ▄████████   ▄▄▄▄███▄▄▄▄      ▄████████ 
  ███    ███   ███    ███ ▄██▀▀▀███▀▀▀██▄   ███    ███ 
  ███    █▀    ███    ███ ███   ███   ███   ███    █▀  
 ▄███          ███    ███ ███   ███   ███  ▄███▄▄▄     
▀▀███ ████▄  ▀███████████ ███   ███   ███ ▀▀███▀▀▀     
  ███    ███   ███    ███ ███   ███   ███   ███    █▄  
  ███    ███   ███    ███ ███   ███   ███   ███    ███ 
  ████████▀    ███    █▀   ▀█   ███   █▀    ██████████ 
                                                       
 ▄██████▄   ▄█    █▄     ▄████████    ▄████████ 
███    ███ ███    ███   ███    ███   ███    ███ 
███    ███ ███    ███   ███    █▀    ███    ███ 
███    ███ ███    ███  ▄███▄▄▄      ▄███▄▄▄▄██▀ 
███    ███ ███    ███ ▀▀███▀▀▀     ▀▀███▀▀▀▀▀   
███    ███ ███    ███   ███    █▄  ▀███████████ 
███    ███ ███    ███   ███    ███   ███    ███ 
 ▀██████▀   ▀██████▀    ██████████   ███    ███ 
                                     ███    ███ 

");
                    }

                    else
                    {
                        WinCount++;
                        Console.ReadLine();
                        Console.WriteLine("All enemies were vanquished! You win! ");
                        Console.ReadLine();
                        Console.WriteLine(@"
▀█████████▄     ▄████████    ▄████████  ▄█    █▄   ▄██████▄  
  ███    ███   ███    ███   ███    ███ ███    ███ ███    ███ 
  ███    ███   ███    ███   ███    ███ ███    ███ ███    ███ 
 ▄███▄▄▄██▀   ▄███▄▄▄▄██▀   ███    ███ ███    ███ ███    ███ 
▀▀███▀▀▀██▄  ▀▀███▀▀▀▀▀   ▀███████████ ███    ███ ███    ███ 
  ███    ██▄ ▀███████████   ███    ███ ███    ███ ███    ███ 
  ███    ███   ███    ███   ███    ███ ███    ███ ███    ███ 
▄█████████▀    ███    ███   ███    █▀   ▀██████▀   ▀██████▀  
               ███    ███                                    

   ▄█    █▄       ▄████████    ▄████████  ▄██████▄  
  ███    ███     ███    ███   ███    ███ ███    ███ 
  ███    ███     ███    █▀    ███    ███ ███    ███ 
 ▄███▄▄▄▄███▄▄  ▄███▄▄▄      ▄███▄▄▄▄██▀ ███    ███ 
▀▀███▀▀▀▀███▀  ▀▀███▀▀▀     ▀▀███▀▀▀▀▀   ███    ███ 
  ███    ███     ███    █▄  ▀███████████ ███    ███ 
  ███    ███     ███    ███   ███    ███ ███    ███ 
  ███    █▀      ██████████   ███    ███  ▀██████▀  
                              ███    ███            

");
                    }
                    
                    // Credits
                    Console.ReadLine();
                    GameOver = true;
                    Reset = true;
                    hero.Health = 0;
                    Console.WriteLine(@" Made by:

 __    __ __    __ 
|  |  |  |  |  |  | 
|  |__|  |  |__|  | 
|   __   |   __   | 
|  |  |  |  |  |  | 
|__|  |__|__|  |__| HH Games 2021

");

                    // Play Again or Quit Option
                    Console.ReadLine();
                    Console.WriteLine($"Win Count: {WinCount}");
                    Console.WriteLine($"Lose Count: {LoseCount}");
                    Console.WriteLine();
                    Console.WriteLine("Press 'Enter' to play again, else input 'stop' to quit");
                    if (Console.ReadLine() == "stop")
                    {
                        playerWantsToQuit = true;
                    }
                }


                // Randomly spawn new units
                static Unit SpawnNewUnit()
                {
                    int random1 = random.Next(0, 6);

                    if (random1 == 0)
                    {
                        Console.WriteLine($@"A wild Necromancer has spawned!

                 /\
                 ||
   ____ (((+))) _||_
  /.--.\  .-.  /.||.\
 /.,   \\(0.0)// || \\
/;`';/\ \\|m|//  ||  ;\
|:   \ \__`:`____||__:|
|:    \__ \T/ (@~)(~@)|
|:    _/|     |\_\/  :|
|:   /  |     |  \   :|
|'  /   |     |   \  '|
 \_/    |     |    \_/
        |     |
        |_____|
        |_____|
");
                        return new Necromancer();
                    }
                    else if (random1 == 1)
                    {
                        
                        Console.WriteLine($@"A wild Skeleton has spawned!

      .-.
     (o.o)
      |=|
     __|__
   //.=|=.\\
  // .=|=. \\
  \\ .=|=. //
   \\(_=_)//
    (:| |:)
     || ||
     () ()
     || ||
     || ||
    ==' '==
");
                        return new Skeleton();
                    }

                    else if (random1 == 2)
                    {
                        Console.WriteLine($@"A wild Bomb has spawned!

             . . .                         
              \|/                          
            `--+--'                        
              /|\                          
             ' | '                         
               |                           
               |                           
           ,--'#`--.                       
           |#######|                       
        _.-'#######`-._                    
     ,-'###############`-.                 
   ,'#####################`,               
  /#########################\              
 |###########################|             
|######## [*] ### [*] ########|            
|#############################|            
|#############################|            
|############# W #############|            
 |###########################|             
  \#########################/              
   `.#####################,'               
     `._###############_,'                 
        `--..#####..--'
");
                        return new Bomb();
                    }

                    else if (random1 == 3)
                    {
                        Console.WriteLine($@"A wild Ghost has spawned!

     .-.
   .'   `.
   :g g   :
   : o    `.
  :         ``.
 :             `.
:  :         .   `.
:   :          ` . `.
 `.. :            `. ``;
    `:;             `:'
       :              `.
        `.              `.     .
          `'`'`'`---..,___`;.-'
");
                        return new Ghost();
                    }

                    else if (random1 == 4)
                    {
                        Console.WriteLine($@"A wild Jackalope has spawned!

                     /\    .-` /
                    /  ; .'  .' 
                   :   :/  .'   
                    \  ;-.'     
       .--""""--..__/     `.    
     .'           .'    `o  \   
    /                    `   ;  
   :                  \       :  
 .-;        -.         `.__.-'  
:  ;          \     ,   ;       
'._:           ;   :   (        
    \/  .__    ;    \   `-.     
     ;     '-,/_..--'`-..__)    
     '""--.._:
");
                        return new Jackalope();
                    }

                    else
                    {
                        Console.WriteLine($@"A wild Hedgehog has spawned!

             \ / \/ \/ / ,
           \ /  \/ \/  \/  / ,
         \ \ \/ \/ \/ \ \/ \/ /
       .\  \/  \/ \/ \/  \/ / / /
      '  / / \/  \/ \/ \/  \/ \ \/ \
   .'     ) \/ \/ \/ \/  \/  \/ \ / \
  /   o    ) \/ \/ \/ \/ \/ \/ \// /
o'_ ',__ .'   ,.,.,.,.,.,.,.,'- '%
             // \\          // \\        
            ''  ''         ''  ''
");
                        return new Hedgehog();
                    }
                }
            }
        }
    }
}

