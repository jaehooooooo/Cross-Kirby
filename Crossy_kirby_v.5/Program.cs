using System;
using System.Threading;
using static System.Console;

namespace Crossy_Kirby_v._2
{
    class Input : GameManager
    {
        public void Input_key()
        {
            while (ingame_start)
            {
                Print print = new Print();
                GameManager gameManager = new GameManager();
                ConsoleKey key = ReadKey().Key;

                if (key == ConsoleKey.UpArrow)
                {
                    GameManager.Uparrow = true;
                }
                if (key == ConsoleKey.Spacebar && GameManager.stage_1_clear)
                {
                    Print.b_attack = true;
                }
                if (key == ConsoleKey.Escape)
                {
                    Ingame_finish = true;
                }
            }
        }
        public void Timewatch() //무한 모드일때 시간 표시하기
        {
            for (int i = 0; i < 30; i++)
            {
                Thread.Sleep(1000);
                if (i % 10 == 0)
                {
                    b_object_2 = true;
                }
            }
            timeover = true;
        }
        public void Stun_print()
        {
            for (int i = 0; i < 2; i++)
            {
                Thread.Sleep(100);
            }
            kirby_damage = false;
        }
    }
    class GameManager
    {
        // 지정된 위치에 String을 작성해주는 함수
        void WriteTool(int x, int y, string s)
        {
            SetCursorPosition(x, y);
            Write(s);
        }
        // 무한모드일때 시간 표시
        public void Timeover()
        {
            Input input = new Input();
            input.Timewatch();
        }
        public const int KIRBY_LIFE = 5, KOOPA_LIFE = 5;
        public static int kirby_life, koopa_life, ate_apple;

        public static bool stage_1_clear, stage_2_clear, gameover, Uparrow , timeover, b_object_2,
                            ingame_start, Ingame_finish, kirby_damage;
        // 게임 초기화
        public void Reset()
        {
            // 플레이어 초기화
            kirby_life = KIRBY_LIFE; koopa_life = KIRBY_LIFE; ate_apple = 0;
            stage_1_clear = false; stage_2_clear = false; gameover = false; Uparrow = false; timeover = false; b_object_2 = false;
            ingame_start = false; Ingame_finish = false; kirby_damage = false;
            // 두번째 오브젝트 비활성화
            Print.b_attack = false;
            // 오브젝트 초기화
            Object.kirby_Match = false; Object_Double.kirby_Match = false;
            Object.object_x = 105; Object_Double.object_x = 105;

            Input input = new Input();
            Print print = new Print();
            // 스레드 타이머 시작
            ingame_start = true;
            Thread time = new Thread(Timeover);
            time.Start();

        }
        // 게임 시작
        public void GameStart()
        {
            Print print = new Print();
            //// 스레드 타이머 시작
            //if 스레드 타이머가 3분을 넘었고 먹은 사과 갯수가 5개 이상일때 => stage_1_clear = true; break; 
            while (!stage_1_clear && !gameover && ingame_start) //스테이지 1 실행
            {
                print.Stage_1_print();  // 스테이지 1 화면 출력

                // 
                if (timeover && ate_apple >= 5 && Object.object_x <= 29)    
                {
                    stage_1_clear = true;
                    break;
                }
                if (kirby_life == 0)    // if 커비의 체력이 0이라면 break;
                {
                    gameover = true;
                    ingame_start = false;
                    break;
                }
                if (Ingame_finish)
                {
                    ForegroundColor = ConsoleColor.Red;
                    BackgroundColor = ConsoleColor.Black;
                    WriteTool(58, 14, "게임을 종료하시겠습니까?");
                    WriteTool(34, 16, "현재 게임에서 나가시려면 ( ENTER )를 두번 눌러주세요,");
                    WriteTool(34, 17, "게임을 계속하시려면 ( SPACE BAR )를 눌러주세요.");

                    ConsoleKey key = ReadKey().Key;
                    if (key == ConsoleKey.Enter) //엔터를 두번 눌러야되는 이유?
                    {
                        gameover = true;
                        Write("게임을 종료합니다.");
                        break;
                    }
                    else
                    {
                        Ingame_finish = false;
                    }
                }
            }
            if (stage_1_clear)
            {
                print.Map_Change();
                print.Nintendo_Low_Screen_Print();
                ForegroundColor = ConsoleColor.Black;
                BackgroundColor = ConsoleColor.White;
                WriteTool(30, 45, "산넘고 물건너 드디어 커비는 쿠파가 있는 성에 도착했다.");
                WriteTool(34, 47, "-엔터를 눌러주세요-");
                ReadKey();
                print.Nintendo_Low_Screen_Print();
                ForegroundColor = ConsoleColor.Black;
                BackgroundColor = ConsoleColor.White;
                WriteTool(58, 45, "쿠궁..!!");
                Thread.Sleep(300);
                WriteTool(58, 46, "나를 쓰러트리면 별동별을 돌려주마!");
                Thread.Sleep(100);
                WriteTool(58, 47, "그럴일은 없겠지만 말이다!");
                WriteTool(34, 49, "-엔터를 눌러주세요-");
                ReadKey();
            }
            if (stage_1_clear && !stage_2_clear && !gameover)
            {
                BackgroundColor = ConsoleColor.Black;
                print.Koopa_print();
            }
            //스테이지 2 실행
            while (stage_1_clear && !stage_2_clear && !gameover) //스테이지 2 실행
            {
                print.Stage_2_print();  // 스테이지 2 화면 출력

                if (kirby_life == 0)    // if 커비의 체력이 0이라면 break;
                {
                    ingame_start = false;
                    gameover = true;
                    break;
                }
                if (koopa_life == 0)
                {
                    ingame_start = false;
                    stage_2_clear = true;
                    break;
                }
                if (Ingame_finish)
                {
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                    WriteTool(58, 14, "게임을 종료하시겠습니까?");
                    WriteTool(34, 16, "현재 게임에서 나가시려면 ( ENTER )를 두번 눌러주세요,");
                    WriteTool(34, 17, "게임을 계속하시려면 ( SPACE BAR )를 눌러주세요.");

                    ConsoleKey key = ReadKey().Key;
                    if (key == ConsoleKey.Enter) //엔터를 두번 눌러야되는 이유?
                    {
                        gameover = true;
                        Write("게임을 종료합니다.");
                        break;
                    }
                    else
                    {
                        Ingame_finish = false;
                    }
                }
            }
        }
        public void GameEnding()
        {
            ingame_start = false;
            Clear();
            Print print = new Print();
            print.Nintendo_print();
            print.Nintendo_Low_Screen_Print();
            if (GameManager.kirby_life == 0 || GameManager.koopa_life == 0)
            {
                if (stage_2_clear) // 스테이지를 다 깼다면
                {
                    print.Win_Page();
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                    WriteTool(58, 45, "축하합니다 커비가 무사히 별동별을 찾았습니다.");
                    WriteTool(34, 46, "ENTER 를 눌러주세요!");
                }
                else // 스테이지 깨는것을 실패했다면
                {
                    print.Lose_Page();
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                    WriteTool(58, 45, "게임오버입니다");
                    WriteTool(34, 46, "ENTER 를 눌러주세요!");
                }
                ConsoleKey key = ReadKey().Key;
            }
        }
        //커비 
        protected bool isjumping = false, isbottom = true;
        protected bool legflag = true;
        protected const int KIRBY_X = 40, KIRBY_Y = 20; //커비 위치 기본값
        protected static int kirby_Y = KIRBY_Y, gravity = 3;
        public void KIrby_Jump()
        {
            if (!kirby_damage)
            {
                if (Uparrow && isbottom)
                {
                    isjumping = true;
                    isbottom = false;
                    Uparrow = false;
                }
                if (isjumping)
                {
                    kirby_Y -= gravity;
                }
                else
                {
                    kirby_Y += gravity * 2;
                }

                if (kirby_Y >= KIRBY_Y)
                {
                    kirby_Y = KIRBY_Y;
                    isbottom = true;
                }
                if (kirby_Y <= 8)
                {
                    isjumping = false;
                }
            }
        }
        public void Kirby_Shape()
        {
            BackgroundColor = ConsoleColor.Magenta;
            ForegroundColor = ConsoleColor.White;
            WriteTool(KIRBY_X, kirby_Y, "  ⊙");
            ForegroundColor = ConsoleColor.Yellow;
            WriteTool(KIRBY_X, kirby_Y + 1, "  ◀");
            BackgroundColor = ConsoleColor.DarkCyan;
            ForegroundColor = ConsoleColor.DarkRed;
            if (legflag)
            {
                WriteTool(KIRBY_X, kirby_Y + 2, "■");
                WriteTool(KIRBY_X + 4, kirby_Y + 2, "■");

                legflag = false;
            }
            else
            {
                WriteTool(KIRBY_X, kirby_Y + 2, "■■");
                legflag = true;
            }
            Console.ResetColor();
        }
        public void Kirby_Shape_Damage()
        {
            ForegroundColor = ConsoleColor.Red;
            WriteTool(KIRBY_X, kirby_Y - 2, " ♥ -1");
            BackgroundColor = ConsoleColor.Blue;
            ForegroundColor = ConsoleColor.White;
            WriteTool(KIRBY_X, kirby_Y, "  X");
            WriteTool(KIRBY_X, kirby_Y + 1, "    ");
            BackgroundColor = ConsoleColor.DarkCyan;
            ForegroundColor = ConsoleColor.DarkBlue;
            if (legflag)
            {
                WriteTool(KIRBY_X, kirby_Y + 2, "■");
                WriteTool(KIRBY_X + 4, kirby_Y + 2, "■");

                legflag = false;
            }
            else
            {
                WriteTool(KIRBY_X, kirby_Y + 2, "■■");
                legflag = true;
            }
            Console.ResetColor();
        }
        public void Kirby_Shape_Eat()
        {
            ForegroundColor = ConsoleColor.Yellow;
            WriteTool(KIRBY_X, kirby_Y - 2, " ♬ ♪");
            BackgroundColor = ConsoleColor.Magenta;
            ForegroundColor = ConsoleColor.Red;
            WriteTool(KIRBY_X, kirby_Y, "  ♥");
            WriteTool(KIRBY_X, kirby_Y + 1, "    ");
            BackgroundColor = ConsoleColor.DarkCyan;
            ForegroundColor = ConsoleColor.DarkRed;
            if (legflag)
            {
                WriteTool(KIRBY_X, kirby_Y + 2, "■");
                WriteTool(KIRBY_X + 4, kirby_Y + 2, "■");

                legflag = false;
            }
            else
            {
                WriteTool(KIRBY_X, kirby_Y + 2, "■■");
                legflag = true;
            }
            Console.ResetColor();
        }
        public void Kirby_print()
        {
            KIrby_Jump();
            Kirby_Shape();
        }

    }
    struct Heart_Icon
    {
        public int x, y;
        public bool minus;
        public void Sample()
        {
            if (!minus)
            {

                BackgroundColor = ConsoleColor.Red;
                WriteTool(x + 2, y - 1, "  ");
                WriteTool(x + 6, y - 1, "  ");
                WriteTool(x, y, "          ");
                WriteTool(x + 2, y + 1, "      ");
                WriteTool(x + 4, y + 2, "  ");
                Console.ResetColor();

                //WriteTool(x, y, "■■");
                //WriteTool(x, y + 1, "■■");
            }
        }
        void WriteTool(int x, int y, string s)
        {
            SetCursorPosition(x, y);
            Write(s);
        }
    }
    struct Koppa_heart_Icon
    {
        public int x, y;
        public bool minus;
        public void Sample()
        {
            if (!minus)
            {
                BackgroundColor = ConsoleColor.Black;
                WriteTool(x + 2, y - 1, "  ");
                WriteTool(x + 6, y - 1, "  ");
                WriteTool(x, y, "          ");
                WriteTool(x + 2, y + 1, "      ");
                WriteTool(x + 4, y + 2, "  ");
                Console.ResetColor();
            }
        }
        void WriteTool(int x, int y, string s)
        {
            SetCursorPosition(x, y);
            Write(s);
        }
    }
    struct Ate_apple_Icon
    {
        public int x, y;
        public bool minus;
        public void Sample()
        {
            if (!minus)
            {
                BackgroundColor = ConsoleColor.Green;
                ForegroundColor = ConsoleColor.White;
                WriteTool(x + 4, y - 1, "♭");
                BackgroundColor = ConsoleColor.White;
                ForegroundColor = ConsoleColor.Yellow;
                WriteTool(x + 2, y, "■■■");
                WriteTool(x, y + 1, "■■■■■");
                WriteTool(x + 2, y + 2, "■■■");
                Console.ResetColor();
            }
        }
        void WriteTool(int x, int y, string s)
        {
            SetCursorPosition(x, y);
            Write(s);
        }
    }

    // 픽셀 아트를 화면에 출력
    class Print : GameManager
    {
        const int FULLSCREEN_X = 26, FULL_SCREEN_END_X = 116;
        const int FULLSCREEN_UP_Y = 4, FULL_SCREEN_UP_END_Y = 26;
        const int FULLSCREEN_DOWN_Y = 36, FULL_SCREEN_DOWN_END_Y = 56;

        public static bool b_attack = false;
        void WriteTool(int x, int y, string s)
        {
            SetCursorPosition(x, y);
            Write(s);
        }
        public void Nintendo_Low_print_Apple()
        {
            Ate_apple_Icon[] icons = new Ate_apple_Icon[ate_apple];
            if (ate_apple > 5)
            {
                ate_apple = 5;
            }
            for (int i = 0; i < ate_apple; i++)
            {
                icons[i].x = i * 13 + 46;
                icons[i].y = 46;
                icons[i].minus = false;
            }
            for (int i = 0; i < ate_apple; i++)
            {
                icons[i].Sample();
                BackgroundColor = ConsoleColor.Gray;
            }

            if (ingame_start)
            {
                BackgroundColor = ConsoleColor.White;
                ForegroundColor = ConsoleColor.Red;
                WriteTool(30, 39, "커비 체력");
                ForegroundColor = ConsoleColor.Black;
                WriteTool(30, 46, "먹은 사과 갯수");
                if (stage_1_clear)
                {
                    ForegroundColor = ConsoleColor.Black;
                    WriteTool(30, 53, "남은 쿠파 체력");
                }
            }
        }
        public void Nintendo_Low_print_Heart()
        {
            Heart_Icon[] icons = new Heart_Icon[kirby_life];
            for (int i = 0; i < kirby_life; i++)
            {

                icons[i].x = i * 13 + 46;
                icons[i].y = 39;
                icons[i].minus = false;
            }

            for (int i = 0; i < kirby_life; i++)
            {
                icons[i].Sample();
            }
        }
        public void Nintendo_Low_print_Koppa_heart()
        {
            Koppa_heart_Icon[] icons = new Koppa_heart_Icon[koopa_life];
            for (int i = 0; i < koopa_life; i++)
            {
                icons[i].x = i * 13 + 46; //46
                icons[i].y = 53;
                icons[i].minus = false;
            }
            for (int i = 0; i < koopa_life; i++)
            {
                icons[i].Sample();
            }
        }
        public void Stage_1_print()
        {
            Object object1 = new Object();
            Object_Double object_Double = new Object_Double();
            Nintendo_Low_Screen_Print();
            Stage_1_BackGround();
            object1.Make_Object();
            if (b_object_2)
            {
                object_Double.Make_Object();
            }
            Nintendo_Low_print_Apple();
            Nintendo_Low_print_Heart();
            if (Object.kirby_Match || Object_Double.kirby_Match)
            {
                BackgroundColor = ConsoleColor.DarkCyan;
                object1.Erase_Object();
                if (Object.kirby_hurt || Object_Double.kirby_hurt)
                {
                    Input input = new Input();
                    Thread stun = new Thread(input.Stun_print);
                    stun.Start();
                    Kirby_Shape_Damage();
                    stun.Join();
                }
                else
                {
                    Input input = new Input();
                    Thread stun = new Thread(input.Stun_print);
                    stun.Start();
                    Kirby_Shape_Eat();
                    stun.Join();

                }
            }
            else Kirby_print();

            Thread.Sleep(150);
            Console.ResetColor();
        }
        public void Stage_2_print()
        {
            Object object1 = new Object();
            Attack attack = new Attack();

            Nintendo_Low_Screen_Print();
            Nintendo_Low_print_Apple();
            Nintendo_Low_print_Heart();
            Nintendo_Low_print_Koppa_heart();
            Stage_2_BackGround();
            object1.Make_Object_2();
            if (Object.attack_time)
            {
                WriteTool(50, 10, "쿠파가 지쳤어 지금이 공격할 타이밍이야!");
            }
            if (b_attack && Object.attack_time)
            {
                attack.Attack_Action();
            }
            if (Object.kirby_Match)
            {
                object1.Erase_Object();
                Input input = new Input();
                Thread stun = new Thread(input.Stun_print);
                stun.Start();
                Kirby_Shape_Damage();
                stun.Join();
            }
            else Kirby_print();
            Thread.Sleep(100);
            //attack.Erase_Attack_shape();
            Console.ResetColor();
        }
        public void Nintendo_print()
        {
            Console.ResetColor();
            #region 닌텐도 모양 // 안쪽 화면 x : 25 ~130쯤 , y : 4~26 / 36~56
            WriteTool(0, 0, "┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            for (int i = 1; i < 30; i++)
            {
                WriteTool(0, i, "┃                                                                                                                                            ┃");
            }
            WriteTool(0, 30, "┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            WriteTool(0, 31, "┃           ┃                                                                                                                    ┃           ┃");
            WriteTool(70, 31, "＠");
            ForegroundColor = ConsoleColor.White;
            WriteTool(134, 31, "┃┃┃");
            Console.ResetColor();
            WriteTool(0, 32, "┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            for (int i = 33; i < 60; i++)
            {
                WriteTool(0, i, "┃                                                                                                                                            ┃");
            }
            WriteTool(0, 60, "┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");

            BackgroundColor = ConsoleColor.White;
            WriteTool(24, 3, "┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓ ");
            for (int i = 4; i < 27; i++)
            {
                WriteTool(24, i, "┃                                                                                           ┃ ");
            }
            WriteTool(24, 27, "┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛ ");

            WriteTool(24, 35, "┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓ ");
            for (int i = 36; i < 57; i++)
            {
                WriteTool(24, i, "┃                                                                                           ┃ ");
            }
            WriteTool(24, 57, "┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛ ");

            ForegroundColor = ConsoleColor.Black;
            WriteTool(10, 40, "┏━━━┓ ");
            WriteTool(10, 41, "┃ ┃ ┃ ");
            WriteTool(6, 42, "┏━━━┛   ┗━━━┓ ");
            WriteTool(6, 43, "┃  ━      ━ ┃ ");
            WriteTool(6, 44, "┗━━━┓   ┏━━━┛ ");
            WriteTool(10, 45, "┃ ┃ ┃ ");
            WriteTool(10, 46, "┗━━━┛ ");
            WriteTool(127, 39, "┏━━━┓ ");
            WriteTool(127, 40, "┃ X ┃ ");
            WriteTool(127, 41, "┗━━━┛ ");
            WriteTool(121, 42, "┏━━━┓ ");
            WriteTool(121, 43, "┃ Y ┃ ");
            WriteTool(121, 44, "┗━━━┛ ");
            WriteTool(133, 42, "┏━━━┓ ");
            WriteTool(133, 43, "┃ A ┃ ");
            WriteTool(133, 44, "┗━━━┛ ");
            WriteTool(127, 45, "┏━━━┓ ");
            WriteTool(127, 46, "┃ B ┃ ");
            WriteTool(127, 47, "┗━━━┛ ");
            Console.ResetColor();
            #endregion
            BackgroundColor = ConsoleColor.White;
            for (int i = 4; i < 27; i++)
            {
                WriteTool(26, i, "                                                                                          ");
            }
            for (int i = 36; i < 57; i++)
            {
                WriteTool(26, i, "                                                                                          ");
            }
            Console.ResetColor();
        }
        public void Stage_1_BackGround()
        {
            BackgroundColor = ConsoleColor.DarkGreen;
            for (int i = 23; i < 27; i++)
            {
                WriteTool(26, i, "                                                                                          ");
            }
            BackgroundColor = ConsoleColor.DarkCyan;
            for (int i = 4; i < 23; i++)
            {
                WriteTool(26, i, "                                                                                          ");
            }
        }
        public void Stage_2_BackGround()
        {
            BackgroundColor = ConsoleColor.Red;
            for (int i = 23; i < 27; i++)
            {
                WriteTool(26, i, "                                                                                          ");
            }
            BackgroundColor = ConsoleColor.Black;
            for (int i = 4; i < 23; i++)
            {
                //WriteTool(26, i, "                                                                                          "); 
                WriteTool(26, i, "                                                                    ");
            }

            Console.ResetColor();
        }
        public void BackGround_print()
        {
            // 쿠파 일러스트 출력
            // 닌텐도 모양 출력

            // 닌텐도 아래부분 화면 출력 메소드
            // 시작 메뉴 출력 메소드
            // 게임 방법 출력 메소드
            // 쿠파 일러스트 출력 메소드
            // 게임 오버 화면 출력 메소드
            // 게임 클리어 화면 출력 메소드
        }
        public void Kirby_Erase()
        {
            BackgroundColor = ConsoleColor.White;
            WriteTool(KIRBY_X, kirby_Y, "      ");
            WriteTool(KIRBY_X, kirby_Y + 1, "      ");
            WriteTool(KIRBY_X, kirby_Y + 2, "      ");
            WriteTool(KIRBY_X, kirby_Y + 2, "      ");
            Console.ResetColor();
        }
        public void Nintendo_High_Screen_Print()
        {
            BackgroundColor = ConsoleColor.White;
            for (int i = 4; i < 27; i++)
            {
                WriteTool(26, i, "                                                                                          ");
            }
            #region 게임시작 화면 위
            //커비
            BackgroundColor = ConsoleColor.Cyan;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 4, "          ");
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 4, "  ");

            BackgroundColor = ConsoleColor.DarkCyan;
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 4, "  ");

            BackgroundColor = ConsoleColor.DarkCyan;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 5, "        ");
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 5, "  ");
            BackgroundColor = ConsoleColor.Cyan;
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 5, "  ");

            BackgroundColor = ConsoleColor.DarkCyan;
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 6, "  ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 6, "  ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 6, "  ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 6, "  ");
            BackgroundColor = ConsoleColor.Cyan;
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 6, "  ");
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 6, "  ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 6, "          ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 6, "  ");

            BackgroundColor = ConsoleColor.DarkCyan;
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 7, "  ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 7, "  ");
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 7, "        ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 7, "  ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 7, "  ");
            BackgroundColor = ConsoleColor.Cyan;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 7, "          ");
            WriteTool(FULLSCREEN_X + 28, FULLSCREEN_UP_Y + 7, "      ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 7, "  ");
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 7, "  ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 7, "  ");

            BackgroundColor = ConsoleColor.Cyan;
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 8, "  ");
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 8, "  ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 8, "  ");
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 8, "  ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 8, "  ");
            BackgroundColor = ConsoleColor.DarkCyan;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 8, "        ");
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 8, "  ");
            WriteTool(FULLSCREEN_X + 28, FULLSCREEN_UP_Y + 8, "    ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 8, "  ");
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 8, "  ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 8, "  ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 8, "  ");

            BackgroundColor = ConsoleColor.Cyan;
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 9, "  ");
            BackgroundColor = ConsoleColor.DarkCyan;
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 9, "  ");

            BackgroundColor = ConsoleColor.Cyan;
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 10, "        ");
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 10, "  ");
            BackgroundColor = ConsoleColor.DarkCyan;
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 10, "  ");

            BackgroundColor = ConsoleColor.DarkCyan;
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 11, "    ");
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 11, "    ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 11, "            ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 11, "    ");

            //Run
            BackgroundColor = ConsoleColor.Cyan;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 13, "          ");

            BackgroundColor = ConsoleColor.Cyan;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 14, "  ");
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 14, "    ");
            BackgroundColor = ConsoleColor.DarkCyan;
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 14, "      ");
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 14, "  ");

            BackgroundColor = ConsoleColor.Cyan;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 15, "  ");
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 15, "  ");
            BackgroundColor = ConsoleColor.DarkCyan;
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 15, "  ");
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 15, "  ");

            BackgroundColor = ConsoleColor.Cyan;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 16, "          ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 16, "  ");
            WriteTool(FULLSCREEN_X + 36, FULLSCREEN_UP_Y + 16, "  ");
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 16, "      ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 16, "  ");

            BackgroundColor = ConsoleColor.DarkCyan;
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 16, "  ");
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 16, "  ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 16, "  ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 16, "  ");

            BackgroundColor = ConsoleColor.Cyan;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 17, "  ");
            WriteTool(FULLSCREEN_X + 20, FULLSCREEN_UP_Y + 17, "  ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 17, "  ");
            WriteTool(FULLSCREEN_X + 36, FULLSCREEN_UP_Y + 17, "  ");
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 17, "  ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 17, "  ");

            BackgroundColor = ConsoleColor.DarkCyan;
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 17, "  ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 17, "      ");
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 17, "  ");
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 17, "  ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 17, "  ");
            WriteTool(FULLSCREEN_X + 44, FULLSCREEN_UP_Y + 17, "    ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 17, "  ");

            BackgroundColor = ConsoleColor.Cyan;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 18, "  ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 18, "  ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 18, "  ");
            WriteTool(FULLSCREEN_X + 36, FULLSCREEN_UP_Y + 18, "  ");
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 18, "  ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 18, "  ");

            BackgroundColor = ConsoleColor.DarkCyan;
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 18, "    ");
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 18, "  ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 18, "  ");
            WriteTool(FULLSCREEN_X + 44, FULLSCREEN_UP_Y + 18, "  ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 18, "  ");

            BackgroundColor = ConsoleColor.Cyan;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 19, "  ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 19, "    ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 19, "        ");
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 19, "  ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 19, "  ");

            BackgroundColor = ConsoleColor.DarkCyan;
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 19, "  ");
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 19, "  ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 19, "  ");
            WriteTool(FULLSCREEN_X + 44, FULLSCREEN_UP_Y + 19, "  ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 19, "  ");

            BackgroundColor = ConsoleColor.DarkCyan;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 20, "    ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 20, "      ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 20, "          ");
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 20, "    ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 20, "    ");

            //커비 형태
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 70, FULLSCREEN_UP_Y + 7, "              ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 68, FULLSCREEN_UP_Y + 8, "                  ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_UP_Y + 9, "          ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 9, "    ");
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_UP_Y + 9, "        ");

            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 86, FULLSCREEN_UP_Y + 9, "  ");

            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 73, FULLSCREEN_UP_Y + 9, " ");
            WriteTool(FULLSCREEN_X + 79, FULLSCREEN_UP_Y + 9, " ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 72, FULLSCREEN_UP_Y + 9, " ");
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_UP_Y + 9, " ");

            //ForegroundColor = ConsoleColor.White; //일단 보류
            //WriteTool(FULLSCREEN_X + 72, FULLSCREEN_UP_Y + 8, "º");
            //WriteTool(FULLSCREEN_X + 78, FULLSCREEN_UP_Y + 8, "o");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_UP_Y + 10, "        ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 10, "    ");
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_UP_Y + 10, "          ");

            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 86, FULLSCREEN_UP_Y + 10, "    ");

            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 72, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_UP_Y + 10, "  ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_UP_Y + 11, "        ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 11, "    ");
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_UP_Y + 11, "          ");

            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 88, FULLSCREEN_UP_Y + 11, "  ");

            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 72, FULLSCREEN_UP_Y + 11, "  ");
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_UP_Y + 11, "  ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_UP_Y + 12, "  ");
            WriteTool(FULLSCREEN_X + 70, FULLSCREEN_UP_Y + 12, "            ");
            WriteTool(FULLSCREEN_X + 86, FULLSCREEN_UP_Y + 12, "  ");

            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 88, FULLSCREEN_UP_Y + 12, "  ");

            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_UP_Y + 12, "    ");
            WriteTool(FULLSCREEN_X + 82, FULLSCREEN_UP_Y + 12, "    ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_UP_Y + 13, "    ");
            WriteTool(FULLSCREEN_X + 72, FULLSCREEN_UP_Y + 13, "  ");
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_UP_Y + 13, "  ");
            WriteTool(FULLSCREEN_X + 82, FULLSCREEN_UP_Y + 13, "    ");

            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 68, FULLSCREEN_UP_Y + 13, "    ");
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_UP_Y + 13, "  ");
            WriteTool(FULLSCREEN_X + 86, FULLSCREEN_UP_Y + 13, "    ");

            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 13, "    ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_UP_Y + 14, "  ");
            WriteTool(FULLSCREEN_X + 70, FULLSCREEN_UP_Y + 14, "  ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 14, "  ");
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_UP_Y + 14, "  ");
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_UP_Y + 14, "    ");

            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 72, FULLSCREEN_UP_Y + 14, "  ");
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_UP_Y + 14, "  ");
            WriteTool(FULLSCREEN_X + 88, FULLSCREEN_UP_Y + 14, "  ");

            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_UP_Y + 14, "  ");

            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_UP_Y + 14, "    ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 14, "    ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 56, FULLSCREEN_UP_Y + 14, "        ");


            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 72, FULLSCREEN_UP_Y + 15, "      ");

            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 72, FULLSCREEN_UP_Y + 15, "  ");
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_UP_Y + 15, "    ");

            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_UP_Y + 15, "        ");
            WriteTool(FULLSCREEN_X + 82, FULLSCREEN_UP_Y + 15, "        ");

            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_UP_Y + 15, "      ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 56, FULLSCREEN_UP_Y + 15, "  ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 16, "        ");

            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_UP_Y + 16, "  ");
            WriteTool(FULLSCREEN_X + 88, FULLSCREEN_UP_Y + 16, "  ");

            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_UP_Y + 16, "        ");
            WriteTool(FULLSCREEN_X + 82, FULLSCREEN_UP_Y + 16, "      ");

            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_UP_Y + 16, "      ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_UP_Y + 16, "    ");

            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_UP_Y + 17, "    ");

            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_UP_Y + 17, "  ");
            WriteTool(FULLSCREEN_X + 88, FULLSCREEN_UP_Y + 17, "  ");

            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 68, FULLSCREEN_UP_Y + 17, "        ");
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_UP_Y + 17, "        ");

            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 62, FULLSCREEN_UP_Y + 17, "    ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 60, FULLSCREEN_UP_Y + 17, "  ");

            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 68, FULLSCREEN_UP_Y + 18, "      ");
            WriteTool(FULLSCREEN_X + 82, FULLSCREEN_UP_Y + 18, "      ");

            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_UP_Y + 18, "      ");
            WriteTool(FULLSCREEN_X + 88, FULLSCREEN_UP_Y + 18, "  ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 18, "        ");

            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 62, FULLSCREEN_UP_Y + 18, "  ");

            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 60, FULLSCREEN_UP_Y + 19, "                              ");
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_UP_Y + 20, "                                ");
            WriteTool(FULLSCREEN_X + 56, FULLSCREEN_UP_Y + 21, "                                ");

            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 21, "              ");
            WriteTool(FULLSCREEN_X + 72, FULLSCREEN_UP_Y + 21, "              ");

            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 68, FULLSCREEN_UP_Y + 21, "    ");
            WriteTool(FULLSCREEN_X + 86, FULLSCREEN_UP_Y + 21, "    ");

            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 22, "          ");

            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 22, "                      ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 22, "      ");
            Console.ResetColor();

            #endregion

        }
        public void Nintendo_Low_Screen_Print()
        {
            BackgroundColor = ConsoleColor.White;
            for (int i = 36; i < 57; i++)
            {
                WriteTool(26, i, "                                                                                          ");
            }
            #region 게임시작 화면 아래
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_DOWN_Y + 2, "        ");
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_DOWN_Y + 3, "  ");
            WriteTool(FULLSCREEN_X + 88, FULLSCREEN_DOWN_Y + 3, "  ");
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_DOWN_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 68, FULLSCREEN_DOWN_Y + 5, "          ");
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_DOWN_Y + 6, "      ");
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_DOWN_Y + 6, "  ");
            WriteTool(FULLSCREEN_X + 62, FULLSCREEN_DOWN_Y + 7, "  ");
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_DOWN_Y + 7, "      ");
            WriteTool(FULLSCREEN_X + 60, FULLSCREEN_DOWN_Y + 8, "  ");
            WriteTool(FULLSCREEN_X + 86, FULLSCREEN_DOWN_Y + 8, "    ");
            WriteTool(FULLSCREEN_X + 60, FULLSCREEN_DOWN_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 68, FULLSCREEN_DOWN_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_DOWN_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_DOWN_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_DOWN_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 70, FULLSCREEN_DOWN_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_DOWN_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_DOWN_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_DOWN_Y + 11, "  ");
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_DOWN_Y + 11, "  ");
            WriteTool(FULLSCREEN_X + 70, FULLSCREEN_DOWN_Y + 11, "  ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_DOWN_Y + 11, "  ");
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_DOWN_Y + 11, "  ");
            WriteTool(FULLSCREEN_X + 56, FULLSCREEN_DOWN_Y + 12, "  ");
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_DOWN_Y + 12, "  ");
            WriteTool(FULLSCREEN_X + 70, FULLSCREEN_DOWN_Y + 12, "  ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_DOWN_Y + 12, "  ");
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_DOWN_Y + 12, "  ");
            WriteTool(FULLSCREEN_X + 56, FULLSCREEN_DOWN_Y + 13, "  ");
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_DOWN_Y + 13, "  ");
            WriteTool(FULLSCREEN_X + 70, FULLSCREEN_DOWN_Y + 13, "  ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_DOWN_Y + 13, "  ");
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_DOWN_Y + 13, "  ");
            WriteTool(FULLSCREEN_X + 56, FULLSCREEN_DOWN_Y + 14, "  ");
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_DOWN_Y + 14, "  ");
            WriteTool(FULLSCREEN_X + 70, FULLSCREEN_DOWN_Y + 14, "  ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_DOWN_Y + 14, "  ");
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_DOWN_Y + 14, "  ");
            WriteTool(FULLSCREEN_X + 56, FULLSCREEN_DOWN_Y + 15, "  ");
            WriteTool(FULLSCREEN_X + 68, FULLSCREEN_DOWN_Y + 15, "  ");
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_DOWN_Y + 15, "  ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_DOWN_Y + 16, "    ");
            WriteTool(FULLSCREEN_X + 62, FULLSCREEN_DOWN_Y + 16, "  ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_DOWN_Y + 16, "    ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_DOWN_Y + 17, "  ");
            WriteTool(FULLSCREEN_X + 56, FULLSCREEN_DOWN_Y + 17, "  ");
            WriteTool(FULLSCREEN_X + 60, FULLSCREEN_DOWN_Y + 17, "      ");
            WriteTool(FULLSCREEN_X + 70, FULLSCREEN_DOWN_Y + 17, "          ");
            WriteTool(FULLSCREEN_X + 82, FULLSCREEN_DOWN_Y + 17, "        ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_DOWN_Y + 18, "  ");
            WriteTool(FULLSCREEN_X + 56, FULLSCREEN_DOWN_Y + 18, "  ");
            WriteTool(FULLSCREEN_X + 70, FULLSCREEN_DOWN_Y + 18, "  ");
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_DOWN_Y + 18, "  ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_DOWN_Y + 19, "    ");
            WriteTool(FULLSCREEN_X + 56, FULLSCREEN_DOWN_Y + 19, "  ");
            WriteTool(FULLSCREEN_X + 72, FULLSCREEN_DOWN_Y + 19, "  ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_DOWN_Y + 20, "    ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_DOWN_Y + 20, "    ");
            #endregion

            Console.ResetColor();
        }
        public void Koopa_print()
        {
            const int KOOPA_START_X = 94;
            const int KOOPA_START_Y = 11;
            BackgroundColor = ConsoleColor.Black;
            for (int i = 4; i < 27; i++)
            {
                WriteTool(94, i, "                      ");
            }
            //WriteTool(KOOPA_START_X, KOOPA_START_Y, "      ■■");
            //WriteTool(KOOPA_START_X, KOOPA_START_Y + 1, "  ■■■■■");
            //WriteTool(KOOPA_START_X, KOOPA_START_Y + 2, "■■■■■■■  ■");
            //WriteTool(KOOPA_START_X, KOOPA_START_Y + 3, "■■■■■■■■■■");
            //WriteTool(KOOPA_START_X, KOOPA_START_Y + 4, "  ■■■■■■■■■");
            //WriteTool(KOOPA_START_X, KOOPA_START_Y + 5, "    ■■■■■■■■■");
            //WriteTool(KOOPA_START_X, KOOPA_START_Y + 6, "      ■■■■■■■■");
            //WriteTool(KOOPA_START_X, KOOPA_START_Y + 7, "■■■■■■■■■■■");
            //WriteTool(KOOPA_START_X, KOOPA_START_Y + 8, "■    ■■■■■■■■");
            //WriteTool(KOOPA_START_X, KOOPA_START_Y + 9, "      ■■■■■■■■");
            //WriteTool(KOOPA_START_X, KOOPA_START_Y + 10, "      ■■■■■■■■");
            //WriteTool(KOOPA_START_X, KOOPA_START_Y + 11, "        ■■■■■■");

            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y - 1, "  ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(KOOPA_START_X + 6, KOOPA_START_Y, "    ");
            WriteTool(KOOPA_START_X + 2, KOOPA_START_Y + 1, "    ");
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 1, "  ");
            WriteTool(KOOPA_START_X + 12, KOOPA_START_Y + 1, "    ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 6, KOOPA_START_Y + 1, "  ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(KOOPA_START_X + 8, KOOPA_START_Y + 1, "    ");
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(KOOPA_START_X + 2, KOOPA_START_Y + 2, "  ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(KOOPA_START_X + 4, KOOPA_START_Y + 2, "    ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(KOOPA_START_X + 4, KOOPA_START_Y + 2, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(KOOPA_START_X + 8, KOOPA_START_Y + 2, "  ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 2, "  ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 2, "    ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(KOOPA_START_X + 16, KOOPA_START_Y + 2, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(KOOPA_START_X, KOOPA_START_Y + 3, "            ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 12, KOOPA_START_Y + 3, "    ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(KOOPA_START_X + 16, KOOPA_START_Y + 3, "  ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(KOOPA_START_X + 18, KOOPA_START_Y + 3, "  ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(KOOPA_START_X + 2, KOOPA_START_Y + 4, "        ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 4, "  ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(KOOPA_START_X + 12, KOOPA_START_Y + 4, "  ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 14, KOOPA_START_Y + 4, "  ");
            BackgroundColor = ConsoleColor.Green;
            WriteTool(KOOPA_START_X + 16, KOOPA_START_Y + 4, "  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(KOOPA_START_X + 18, KOOPA_START_Y + 4, "  ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(KOOPA_START_X + 4, KOOPA_START_Y + 5, "    ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(KOOPA_START_X + 8, KOOPA_START_Y + 5, "  ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 5, "  ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 12, KOOPA_START_Y + 5, "  ");
            BackgroundColor = ConsoleColor.Green;
            WriteTool(KOOPA_START_X + 14, KOOPA_START_Y + 5, "      ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(KOOPA_START_X + 20, KOOPA_START_Y + 5, "  ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(KOOPA_START_X + 6, KOOPA_START_Y + 6, "    ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 6, "  ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 12, KOOPA_START_Y + 6, "  ");
            BackgroundColor = ConsoleColor.Green;
            WriteTool(KOOPA_START_X + 14, KOOPA_START_Y + 6, "  ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(KOOPA_START_X + 16, KOOPA_START_Y + 6, "  ");
            BackgroundColor = ConsoleColor.Green;
            WriteTool(KOOPA_START_X + 18, KOOPA_START_Y + 6, "  ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 20, KOOPA_START_Y + 6, "  ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(KOOPA_START_X, KOOPA_START_Y + 7, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(KOOPA_START_X + 2, KOOPA_START_Y + 7, "    ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(KOOPA_START_X + 6, KOOPA_START_Y + 7, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(KOOPA_START_X + 8, KOOPA_START_Y + 7, "  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 7, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(KOOPA_START_X + 12, KOOPA_START_Y + 7, "  ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(KOOPA_START_X + 14, KOOPA_START_Y + 7, "  ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 16, KOOPA_START_Y + 7, "  ");
            BackgroundColor = ConsoleColor.Green;
            WriteTool(KOOPA_START_X + 18, KOOPA_START_Y + 7, "    ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(KOOPA_START_X + 22, KOOPA_START_Y + 7, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(KOOPA_START_X, KOOPA_START_Y + 8, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(KOOPA_START_X + 4, KOOPA_START_Y + 8, "  ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(KOOPA_START_X + 6, KOOPA_START_Y + 8, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(KOOPA_START_X + 8, KOOPA_START_Y + 8, "  ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 8, "  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(KOOPA_START_X + 12, KOOPA_START_Y + 8, "    ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(KOOPA_START_X + 16, KOOPA_START_Y + 8, "  ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 18, KOOPA_START_Y + 8, "    ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(KOOPA_START_X + 6, KOOPA_START_Y + 9, "  ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(KOOPA_START_X + 8, KOOPA_START_Y + 9, "  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 9, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 9, "        ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(KOOPA_START_X + 16, KOOPA_START_Y + 9, "  ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 18, KOOPA_START_Y + 9, "  ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(KOOPA_START_X + 20, KOOPA_START_Y + 9, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(KOOPA_START_X + 8, KOOPA_START_Y + 10, "    ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(KOOPA_START_X + 14, KOOPA_START_Y + 10, "      ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(KOOPA_START_X + 18, KOOPA_START_Y + 10, "  ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(KOOPA_START_X + 4, KOOPA_START_Y + 11, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(KOOPA_START_X + 6, KOOPA_START_Y + 11, "    ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 11, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(KOOPA_START_X + 12, KOOPA_START_Y + 11, "      ");
        }
        public void Koopa_damage_print()
        {
            const int KOOPA_START_X = 94;
            const int KOOPA_START_Y = 11;
            BackgroundColor = ConsoleColor.Black;
            for (int i = 4; i < 22; i++)
            {
                WriteTool(94, i, "                      ");
            }
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y - 1, "  ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(KOOPA_START_X + 6, KOOPA_START_Y, "    ");
            WriteTool(KOOPA_START_X + 2, KOOPA_START_Y + 1, "    ");
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 1, "  ");
            WriteTool(KOOPA_START_X + 12, KOOPA_START_Y + 1, "    ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 6, KOOPA_START_Y + 1, "  ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(KOOPA_START_X + 8, KOOPA_START_Y + 1, "    ");
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y, "  ");
            BackgroundColor = ConsoleColor.DarkBlue;
            WriteTool(KOOPA_START_X + 2, KOOPA_START_Y + 2, "  ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(KOOPA_START_X + 4, KOOPA_START_Y + 2, "    ");
            BackgroundColor = ConsoleColor.Cyan;
            WriteTool(KOOPA_START_X + 4, KOOPA_START_Y + 2, "  ");
            BackgroundColor = ConsoleColor.DarkBlue;
            WriteTool(KOOPA_START_X + 8, KOOPA_START_Y + 2, "  ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 2, "  ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 2, "    ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(KOOPA_START_X + 16, KOOPA_START_Y + 2, "  ");
            BackgroundColor = ConsoleColor.DarkBlue;
            WriteTool(KOOPA_START_X, KOOPA_START_Y + 3, "            ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 12, KOOPA_START_Y + 3, "    ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(KOOPA_START_X + 16, KOOPA_START_Y + 3, "  ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(KOOPA_START_X + 18, KOOPA_START_Y + 3, "  ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(KOOPA_START_X + 2, KOOPA_START_Y + 4, "        ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 4, "  ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(KOOPA_START_X + 12, KOOPA_START_Y + 4, "  ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 14, KOOPA_START_Y + 4, "  ");
            BackgroundColor = ConsoleColor.Green;
            WriteTool(KOOPA_START_X + 16, KOOPA_START_Y + 4, "  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(KOOPA_START_X + 18, KOOPA_START_Y + 4, "  ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(KOOPA_START_X + 4, KOOPA_START_Y + 5, "    ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(KOOPA_START_X + 8, KOOPA_START_Y + 5, "  ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 5, "  ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 12, KOOPA_START_Y + 5, "  ");
            BackgroundColor = ConsoleColor.Green;
            WriteTool(KOOPA_START_X + 14, KOOPA_START_Y + 5, "      ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(KOOPA_START_X + 20, KOOPA_START_Y + 5, "  ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(KOOPA_START_X + 6, KOOPA_START_Y + 6, "    ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 6, "  ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 12, KOOPA_START_Y + 6, "  ");
            BackgroundColor = ConsoleColor.Green;
            WriteTool(KOOPA_START_X + 14, KOOPA_START_Y + 6, "  ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(KOOPA_START_X + 16, KOOPA_START_Y + 6, "  ");
            BackgroundColor = ConsoleColor.Green;
            WriteTool(KOOPA_START_X + 18, KOOPA_START_Y + 6, "  ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 20, KOOPA_START_Y + 6, "  ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(KOOPA_START_X, KOOPA_START_Y + 7, "  ");
            BackgroundColor = ConsoleColor.DarkBlue;
            WriteTool(KOOPA_START_X + 2, KOOPA_START_Y + 7, "    ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(KOOPA_START_X + 6, KOOPA_START_Y + 7, "  ");
            BackgroundColor = ConsoleColor.DarkBlue;
            WriteTool(KOOPA_START_X + 8, KOOPA_START_Y + 7, "  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 7, "  ");
            BackgroundColor = ConsoleColor.DarkBlue;
            WriteTool(KOOPA_START_X + 12, KOOPA_START_Y + 7, "  ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(KOOPA_START_X + 14, KOOPA_START_Y + 7, "  ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 16, KOOPA_START_Y + 7, "  ");
            BackgroundColor = ConsoleColor.Green;
            WriteTool(KOOPA_START_X + 18, KOOPA_START_Y + 7, "    ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(KOOPA_START_X + 22, KOOPA_START_Y + 7, "  ");
            BackgroundColor = ConsoleColor.DarkBlue;
            WriteTool(KOOPA_START_X, KOOPA_START_Y + 8, "  ");
            BackgroundColor = ConsoleColor.DarkBlue;
            WriteTool(KOOPA_START_X + 4, KOOPA_START_Y + 8, "  ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(KOOPA_START_X + 6, KOOPA_START_Y + 8, "  ");
            BackgroundColor = ConsoleColor.DarkBlue;
            WriteTool(KOOPA_START_X + 8, KOOPA_START_Y + 8, "  ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 8, "  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(KOOPA_START_X + 12, KOOPA_START_Y + 8, "    ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(KOOPA_START_X + 16, KOOPA_START_Y + 8, "  ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 18, KOOPA_START_Y + 8, "    ");
            BackgroundColor = ConsoleColor.DarkBlue;
            WriteTool(KOOPA_START_X + 6, KOOPA_START_Y + 9, "  ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(KOOPA_START_X + 8, KOOPA_START_Y + 9, "  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 9, "  ");
            BackgroundColor = ConsoleColor.DarkBlue;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 9, "        ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(KOOPA_START_X + 16, KOOPA_START_Y + 9, "  ");
            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(KOOPA_START_X + 18, KOOPA_START_Y + 9, "  ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(KOOPA_START_X + 20, KOOPA_START_Y + 9, "  ");
            BackgroundColor = ConsoleColor.DarkBlue;
            WriteTool(KOOPA_START_X + 8, KOOPA_START_Y + 10, "    ");
            BackgroundColor = ConsoleColor.DarkBlue;
            WriteTool(KOOPA_START_X + 14, KOOPA_START_Y + 10, "      ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(KOOPA_START_X + 18, KOOPA_START_Y + 10, "  ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(KOOPA_START_X + 4, KOOPA_START_Y + 11, "  ");
            BackgroundColor = ConsoleColor.DarkBlue;
            WriteTool(KOOPA_START_X + 6, KOOPA_START_Y + 11, "    ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(KOOPA_START_X + 10, KOOPA_START_Y + 11, "  ");
            BackgroundColor = ConsoleColor.DarkBlue;
            WriteTool(KOOPA_START_X + 12, KOOPA_START_Y + 11, "      ");
        }
        public void Map_Change()
        {
            BackgroundColor = ConsoleColor.Black;
            for (int i = 4; i < 27; i++)
            {
                WriteTool(26, i, "                                                                                          ");
            }
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 20, FULLSCREEN_UP_Y, "  ");
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_UP_Y, "  ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y, "  ");
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 82, FULLSCREEN_UP_Y, "  ");
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 86, FULLSCREEN_UP_Y, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 1, "  ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 1, "    ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 1, "              ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 70, FULLSCREEN_UP_Y + 1, "      ");
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 82, FULLSCREEN_UP_Y + 1, "        ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 2, "  ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 2, "  ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 2, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 2, "                    ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_UP_Y + 2, "  ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 68, FULLSCREEN_UP_Y + 2, "  ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 70, FULLSCREEN_UP_Y + 2, "  ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 72, FULLSCREEN_UP_Y + 2, "  ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 2, "      ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 3, "    ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 3, "                        ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 3, "          ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 3, "  ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_UP_Y + 3, "  ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 68, FULLSCREEN_UP_Y + 3, "    ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 72, FULLSCREEN_UP_Y + 3, "  ");
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_UP_Y + 3, "  ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 3, "      ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 86, FULLSCREEN_UP_Y + 3, "  ");
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X, FULLSCREEN_UP_Y + 4, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 20, FULLSCREEN_UP_Y + 4, "                          ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 4, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 4, "                ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 68, FULLSCREEN_UP_Y + 4, "    ");
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 4, "      ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 4, "      ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X, FULLSCREEN_UP_Y + 5, "    ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 5, "                  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 36, FULLSCREEN_UP_Y + 5, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 5, "    ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 5, "  ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 44, FULLSCREEN_UP_Y + 5, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 5, "                    ");
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 5, "      ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 5, "      ");
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X, FULLSCREEN_UP_Y + 6, "      ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 6, "  ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 6, "      ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 6, "            ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 6, "  ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 36, FULLSCREEN_UP_Y + 6, "  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 6, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 6, "  ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 6, "  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 44, FULLSCREEN_UP_Y + 6, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 6, "  ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 6, "    ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 6, "              ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 86, FULLSCREEN_UP_Y + 6, "  ");

            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_UP_Y + 6, "  ");
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_UP_Y + 6, "  ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 6, "  ");
            WriteTool(FULLSCREEN_X + 88, FULLSCREEN_UP_Y + 6, "  ");
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X, FULLSCREEN_UP_Y + 7, "      ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 6, FULLSCREEN_UP_Y + 7, "    ");
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 7, "    ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 7, "  ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 7, "      ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 7, "              ");
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 7, "              ");

            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 7, "  ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 7, "  ");
            WriteTool(FULLSCREEN_X + 44, FULLSCREEN_UP_Y + 7, "  ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 36, FULLSCREEN_UP_Y + 7, "  ");
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 7, "  ");

            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 7, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 7, "  ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 7, "        ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 56, FULLSCREEN_UP_Y + 7, "          ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_UP_Y + 7, "  ");
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_UP_Y + 7, "          ");

            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X, FULLSCREEN_UP_Y + 8, "  ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 2, FULLSCREEN_UP_Y + 8, "    ");
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 6, FULLSCREEN_UP_Y + 8, "        ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 8, "        ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 8, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 8, "          ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 8, "              ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 8, "  ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 8, "  ");
            WriteTool(FULLSCREEN_X + 44, FULLSCREEN_UP_Y + 8, "  ");
            BackgroundColor = ConsoleColor.Cyan;
            WriteTool(FULLSCREEN_X + 36, FULLSCREEN_UP_Y + 8, "  ");
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 8, "  ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 8, "  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 8, "      ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 8, "    ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_UP_Y + 8, "        ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 68, FULLSCREEN_UP_Y + 8, "    ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_UP_Y + 8, "  ");
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 82, FULLSCREEN_UP_Y + 8, "  ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 82, FULLSCREEN_UP_Y + 8, "        ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 9, "            ");
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 9, "          ");
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 9, "    ");
            WriteTool(FULLSCREEN_X + 60, FULLSCREEN_UP_Y + 9, "    ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 44, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 9, "      ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 9, "    ");
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_UP_Y + 9, "  ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_UP_Y + 9, "    ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 9, "    ");
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 9, "      ");
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 4, FULLSCREEN_UP_Y + 9, "        ");
            BackgroundColor = ConsoleColor.Cyan;
            WriteTool(FULLSCREEN_X + 36, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 9, "  ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 9, "  ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 10, "        ");
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 10, "    ");
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 10, "    ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 10, "    ");
            WriteTool(FULLSCREEN_X + 44, FULLSCREEN_UP_Y + 10, "    ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 60, FULLSCREEN_UP_Y + 10, "    ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 35, FULLSCREEN_UP_Y + 10, "    ");
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 10, "   ");
            WriteTool(FULLSCREEN_X + 56, FULLSCREEN_UP_Y + 10, "  ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 10, "    ");
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_UP_Y + 10, "  ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_UP_Y + 10, "      ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 10, "  ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 72, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 10, "      ");
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 10, "            ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 10, "  ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 11, "            ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 11, "            ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 11, "    ");
            WriteTool(FULLSCREEN_X + 60, FULLSCREEN_UP_Y + 11, "  ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 11, "        ");
            WriteTool(FULLSCREEN_X + 56, FULLSCREEN_UP_Y + 11, "    ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 62, FULLSCREEN_UP_Y + 11, "    ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_UP_Y + 11, "    ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 11, "    ");
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 11, "      ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 11, "      ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 11, "      ");
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 11, "          ");
            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 11, "  ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 20, FULLSCREEN_UP_Y + 12, "      ");
            WriteTool(FULLSCREEN_X + 28, FULLSCREEN_UP_Y + 12, "            ");
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 12, "            ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 12, "          ");
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 12, "  ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 12, "      ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 12, "  ");
            WriteTool(FULLSCREEN_X + 62, FULLSCREEN_UP_Y + 12, "    ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 6, FULLSCREEN_UP_Y + 12, "    ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 12, "      ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 12, "      ");
            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 12, "      ");

            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 13, "  ");
            WriteTool(FULLSCREEN_X + 6, FULLSCREEN_UP_Y + 13, "      ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 2, FULLSCREEN_UP_Y + 13, "    ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 13, "              ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 13, "      ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 13, "  ");
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 13, "              ");
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 13, "              ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 13, "            ");
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 13, "  ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 13, "  ");

            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 2, FULLSCREEN_UP_Y + 14, "            ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 14, "  ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 14, "              ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 14, "      ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 20, FULLSCREEN_UP_Y + 14, "  ");
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 14, "                              ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 14, "        ");
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 14, "  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 14, "  ");

            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 2, FULLSCREEN_UP_Y + 15, "            ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 15, "  ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 15, "      ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 15, "      ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 20, FULLSCREEN_UP_Y + 15, "  ");
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 15, "                              ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 15, "      ");
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 15, "  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 15, "  ");

            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 16, "                ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 16, "    ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 16, "      ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 16, "      ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 16, "                              ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 16, "    ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 16, "      ");

            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 17, "            ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 17, "      ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 17, "      ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 17, "      ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 28, FULLSCREEN_UP_Y + 17, "                          ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 17, "            ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 17, "    ");

            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 18, "          ");
            WriteTool(FULLSCREEN_X + 56, FULLSCREEN_UP_Y + 18, "        ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 18, "      ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 18, "      ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 18, "                      ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 18, "            ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 18, "        ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 18, "    ");

            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 19, "      ");
            WriteTool(FULLSCREEN_X + 60, FULLSCREEN_UP_Y + 19, "    ");
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_UP_Y + 19, "  ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 6, FULLSCREEN_UP_Y + 19, "  ");
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_UP_Y + 19, "  ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 19, "      ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 19, "      ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 36, FULLSCREEN_UP_Y + 19, "              ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 19, "                  ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 8, FULLSCREEN_UP_Y + 19, "          ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 19, "          ");

            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 20, "  ");
            WriteTool(FULLSCREEN_X + 60, FULLSCREEN_UP_Y + 20, "  ");
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_UP_Y + 20, "      ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 2, FULLSCREEN_UP_Y + 20, "    ");
            WriteTool(FULLSCREEN_X + 62, FULLSCREEN_UP_Y + 20, "  ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 20, "      ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 20, "      ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 20, "          ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 20, "                ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 20, "        ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 20, "        ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 6, FULLSCREEN_UP_Y + 20, "    ");
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 20, "    ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 20, "    ");

            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 2, FULLSCREEN_UP_Y + 21, "      ");
            WriteTool(FULLSCREEN_X + 60, FULLSCREEN_UP_Y + 21, "            ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 21, "  ");
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 21, "      ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 21, "      ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 21, "                ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 21, "      ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 8, FULLSCREEN_UP_Y + 21, "              ");
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 21, "            ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 21, "    ");
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 21, "    ");

            BackgroundColor = ConsoleColor.Gray;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 22, "          ");
            WriteTool(FULLSCREEN_X + 62, FULLSCREEN_UP_Y + 22, "              ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 22, "  ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 22, "  ");
            WriteTool(FULLSCREEN_X + 28, FULLSCREEN_UP_Y + 22, "  ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 22, "        ");
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_UP_Y + 22, "    ");
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_UP_Y + 22, "    ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 22, "      ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 22, "          ");
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 22, "    ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 22, "        ");
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 22, "            ");

            Console.ResetColor();
        }
        public void Win_Page()
        {
            BackgroundColor = ConsoleColor.Cyan;
            for (int i = 4; i < 27; i++)
            {
                WriteTool(26, i, "                                                                                          ");
            }

            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 1, "      ");
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 1, "  ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 1, "      ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_UP_Y + 1, "  ");
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_UP_Y + 1, "  ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 1, "  ");

            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 2, "        ");
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 2, "            ");
            WriteTool(FULLSCREEN_X + 36, FULLSCREEN_UP_Y + 2, "  ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_UP_Y + 2, "          ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 2, "              ");

            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 2, "        ");
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 2, "            ");
            WriteTool(FULLSCREEN_X + 36, FULLSCREEN_UP_Y + 2, "  ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_UP_Y + 2, "          ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 2, "              ");

            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 4, FULLSCREEN_UP_Y + 3, "  ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 3, "          ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 72, FULLSCREEN_UP_Y + 3, "  ");
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_UP_Y + 3, "      ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 3, "                  ");

            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 4, "    ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_UP_Y + 4, "      ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 4, "                      ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 4, "      ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 36, FULLSCREEN_UP_Y + 4, "  ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_UP_Y + 4, "  ");

            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 6, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 5, "  ");
            BackgroundColor = ConsoleColor.DarkGray;
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 5, "    ");
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_UP_Y + 5, "      ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 5, "                          ");
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 5, "    ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 5, "    ");

            BackgroundColor = ConsoleColor.Green;
            WriteTool(FULLSCREEN_X + 72, FULLSCREEN_UP_Y + 6, "              ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 68, FULLSCREEN_UP_Y + 6, "  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 20, FULLSCREEN_UP_Y + 6, "  ");
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 6, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 6, "          ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 6, "    ");
            WriteTool(FULLSCREEN_X + 28, FULLSCREEN_UP_Y + 6, "          ");
            WriteTool(FULLSCREEN_X + 44, FULLSCREEN_UP_Y + 6, "  ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 6, "      ");

            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 7, "  ");
            WriteTool(FULLSCREEN_X + 20, FULLSCREEN_UP_Y + 7, "  ");
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 7, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 74, FULLSCREEN_UP_Y + 7, "          ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_UP_Y + 7, "    ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 7, "  ");
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 7, "  ");
            WriteTool(FULLSCREEN_X + 28, FULLSCREEN_UP_Y + 7, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 7, "        ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 7, "  ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 7, "             ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 7, "      ");

            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 4, FULLSCREEN_UP_Y + 8, "  ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(FULLSCREEN_X + 20, FULLSCREEN_UP_Y + 8, "  ");
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 8, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 76, FULLSCREEN_UP_Y + 8, "      ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_UP_Y + 8, "      ");
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_UP_Y + 8, "  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 8, "  ");
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 8, "  ");
            WriteTool(FULLSCREEN_X + 28, FULLSCREEN_UP_Y + 8, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 8, "        ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 8, "  ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 8, "            ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 8, "    ");

            BackgroundColor = ConsoleColor.Blue;
            WriteTool(FULLSCREEN_X + 20, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 9, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 78, FULLSCREEN_UP_Y + 9, "  ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 9, "    ");
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_UP_Y + 9, "  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 28, FULLSCREEN_UP_Y + 9, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 9, "        ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 9, "  ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 9, "          ");

            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 4, FULLSCREEN_UP_Y + 10, "      ");
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 10, "      ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 10, "    ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 10, "        ");
            WriteTool(FULLSCREEN_X + 62, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_UP_Y + 10, "  ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 20, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 10, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 10, "    ");
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 10, "    ");
            WriteTool(FULLSCREEN_X + 28, FULLSCREEN_UP_Y + 10, "  ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 10, "      ");

            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 6, FULLSCREEN_UP_Y + 11, "    ");
            WriteTool(FULLSCREEN_X + 36, FULLSCREEN_UP_Y + 11, "    ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 4, FULLSCREEN_UP_Y + 11, "  ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 11, "        ");
            WriteTool(FULLSCREEN_X + 60, FULLSCREEN_UP_Y + 11, "  ");
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_UP_Y + 11, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 11, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 11, "        ");
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 11, "                  ");

            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 8, FULLSCREEN_UP_Y + 12, "  ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 12, "  ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 6, FULLSCREEN_UP_Y + 12, "  ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 12, "    ");
            WriteTool(FULLSCREEN_X + 36, FULLSCREEN_UP_Y + 12, "      ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 12, "  ");
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_UP_Y + 12, "  ");
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_UP_Y + 12, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 44, FULLSCREEN_UP_Y + 12, "    ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 12, "          ");
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 12, "        ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 12, "  ");

            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 13, "  ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 8, FULLSCREEN_UP_Y + 13, "  ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 13, "  ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 13, "        ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 13, "    ");
            WriteTool(FULLSCREEN_X + 62, FULLSCREEN_UP_Y + 13, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 13, "    ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 13, "        ");
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 13, "        ");
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 13, "        ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 13, "  ");

            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 14, "  ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 14, "      ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 6, FULLSCREEN_UP_Y + 14, "    ");
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 14, "      ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 14, "    ");
            WriteTool(FULLSCREEN_X + 60, FULLSCREEN_UP_Y + 14, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 14, "    ");
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 14, "            ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 14, "    ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 14, "  ");

            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 15, "        ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 4, FULLSCREEN_UP_Y + 15, "      ");
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 15, "        ");
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_UP_Y + 15, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 15, "      ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 15, "  ");
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 15, "              ");

            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 16, "    ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 2, FULLSCREEN_UP_Y + 16, "        ");
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 16, "          ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 16, "          ");
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_UP_Y + 16, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 16, "          ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 10, FULLSCREEN_UP_Y + 16, "    ");

            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_UP_Y + 17, "      ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 17, "            ");
            WriteTool(FULLSCREEN_X + 24, FULLSCREEN_UP_Y + 17, "                        ");
            WriteTool(FULLSCREEN_X + 56, FULLSCREEN_UP_Y + 17, "  ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 17, "    ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 17, "    ");
            WriteTool(FULLSCREEN_X + 20, FULLSCREEN_UP_Y + 17, "    ");

            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 82, FULLSCREEN_UP_Y + 18, "      ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 0, FULLSCREEN_UP_Y + 18, "      ");
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 18, "  ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 18, "            ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 18, "            ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 18, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 6, FULLSCREEN_UP_Y + 18, "      ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 18, "    ");
            BackgroundColor = ConsoleColor.DarkMagenta;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 18, "        ");

            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 72, FULLSCREEN_UP_Y + 19, "      ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 19, "                  ");
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 19, "          ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 19, "  ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 2, FULLSCREEN_UP_Y + 19, "            ");
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 19, "          ");

            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 2, FULLSCREEN_UP_Y + 20, "  ");
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_UP_Y + 20, "                ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 20, "              ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 20, "  ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 20, "    ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 20, "              ");

            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 8, FULLSCREEN_UP_Y + 21, "  ");
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_UP_Y + 21, "  ");
            WriteTool(FULLSCREEN_X + 68, FULLSCREEN_UP_Y + 21, "  ");
            WriteTool(FULLSCREEN_X + 72, FULLSCREEN_UP_Y + 21, "        ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 21, "              ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 21, "  ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 21, "    ");

            BackgroundColor = ConsoleColor.White;
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 22, "  ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 22, "  ");
            BackgroundColor = ConsoleColor.Yellow;
            WriteTool(FULLSCREEN_X + 16, FULLSCREEN_UP_Y + 22, "            ");
            BackgroundColor = ConsoleColor.DarkYellow;
            WriteTool(FULLSCREEN_X + 14, FULLSCREEN_UP_Y + 22, "  ");
            WriteTool(FULLSCREEN_X + 28, FULLSCREEN_UP_Y + 22, "    ");

            Console.ResetColor();

        }
        public void Lose_Page()
        {
            BackgroundColor = ConsoleColor.Black;
            for (int i = 4; i < 27; i++)
            {
                WriteTool(26, i, "                                                                                          ");
            }

            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 2, FULLSCREEN_UP_Y + 1, "      ");
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 1, "      ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 1, "  ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 1, "  ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 1, "        ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 1, "    ");
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_UP_Y + 1, "  ");
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_UP_Y + 1, "  ");
            WriteTool(FULLSCREEN_X + 70, FULLSCREEN_UP_Y + 1, "        ");
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_UP_Y + 1, "        ");

            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 2, FULLSCREEN_UP_Y + 2, "  ");
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 2, "  ");
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 2, "  ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 2, "    ");
            WriteTool(FULLSCREEN_X + 28, FULLSCREEN_UP_Y + 2, "    ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 2, "  ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 2, "  ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 2, "  ");
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_UP_Y + 2, "  ");
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_UP_Y + 2, "  ");
            WriteTool(FULLSCREEN_X + 70, FULLSCREEN_UP_Y + 2, "  ");
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_UP_Y + 2, "  ");
            WriteTool(FULLSCREEN_X + 86, FULLSCREEN_UP_Y + 2, "  ");

            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 2, FULLSCREEN_UP_Y + 3, "  ");
            WriteTool(FULLSCREEN_X + 6, FULLSCREEN_UP_Y + 3, "    ");
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 3, "        ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 3, "  ");
            WriteTool(FULLSCREEN_X + 26, FULLSCREEN_UP_Y + 3, "  ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 3, "  ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 3, "        ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 3, "  ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 3, "  ");
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_UP_Y + 3, "  ");
            WriteTool(FULLSCREEN_X + 66, FULLSCREEN_UP_Y + 3, "  ");
            WriteTool(FULLSCREEN_X + 70, FULLSCREEN_UP_Y + 3, "        ");
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_UP_Y + 3, "        ");

            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 2, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 8, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 60, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 64, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 70, FULLSCREEN_UP_Y + 4, "  ");
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_UP_Y + 4, "    ");

            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 2, FULLSCREEN_UP_Y + 5, "    ");
            WriteTool(FULLSCREEN_X + 8, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 12, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 18, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 22, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 5, "        ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 62, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 70, FULLSCREEN_UP_Y + 5, "        ");
            WriteTool(FULLSCREEN_X + 80, FULLSCREEN_UP_Y + 5, "  ");
            WriteTool(FULLSCREEN_X + 84, FULLSCREEN_UP_Y + 5, "    ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 7, "            ");
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 8, "                  ");
            WriteTool(FULLSCREEN_X + 36, FULLSCREEN_UP_Y + 9, "                      ");
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 10, "                          ");
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 11, "                              ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 12, "          ");
            WriteTool(FULLSCREEN_X + 44, FULLSCREEN_UP_Y + 12, "        ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 12, "          ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 12, "  ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 12, "  ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 13, "        ");
            WriteTool(FULLSCREEN_X + 44, FULLSCREEN_UP_Y + 13, "        ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 13, "        ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 13, "  ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 13, "  ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(FULLSCREEN_X + 42, FULLSCREEN_UP_Y + 13, "  ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 13, "  ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 14, "      ");
            WriteTool(FULLSCREEN_X + 44, FULLSCREEN_UP_Y + 14, "      ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 14, "        ");
            BackgroundColor = ConsoleColor.Black;
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 14, "  ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 14, "  ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 14, "    ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 14, "    ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 34, FULLSCREEN_UP_Y + 15, "    ");
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 15, "  ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 46, FULLSCREEN_UP_Y + 15, "  ");
            WriteTool(FULLSCREEN_X + 48, FULLSCREEN_UP_Y + 15, "  ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 15, "    ");
            WriteTool(FULLSCREEN_X + 56, FULLSCREEN_UP_Y + 15, "      ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 15, "      ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 15, "      ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 44, FULLSCREEN_UP_Y + 16, "      ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 16, "      ");
            WriteTool(FULLSCREEN_X + 58, FULLSCREEN_UP_Y + 16, "    ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 36, FULLSCREEN_UP_Y + 16, "  ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 16, "    ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 16, "      ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 16, "    ");

            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 44, FULLSCREEN_UP_Y + 17, "      ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 17, "        ");
            WriteTool(FULLSCREEN_X + 54, FULLSCREEN_UP_Y + 17, "        ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 17, "  ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 17, "  ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 17, "    ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 17, "  ");

            BackgroundColor = ConsoleColor.DarkGreen;
            WriteTool(FULLSCREEN_X, FULLSCREEN_UP_Y + 18, "                                                                                          ");
            BackgroundColor = ConsoleColor.Magenta;
            WriteTool(FULLSCREEN_X + 44, FULLSCREEN_UP_Y + 18, "      ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 30, FULLSCREEN_UP_Y + 18, "        ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 18, "        ");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(FULLSCREEN_X + 38, FULLSCREEN_UP_Y + 18, "  ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 18, "  ");
            BackgroundColor = ConsoleColor.Blue;
            WriteTool(FULLSCREEN_X + 40, FULLSCREEN_UP_Y + 18, "    ");
            WriteTool(FULLSCREEN_X + 50, FULLSCREEN_UP_Y + 18, "  ");

            BackgroundColor = ConsoleColor.Blue;
            WriteTool(FULLSCREEN_X, FULLSCREEN_UP_Y + 19, "                                                                                          ");
            BackgroundColor = ConsoleColor.DarkRed;
            WriteTool(FULLSCREEN_X + 32, FULLSCREEN_UP_Y + 19, "      ");
            WriteTool(FULLSCREEN_X + 52, FULLSCREEN_UP_Y + 19, "      ");

            BackgroundColor = ConsoleColor.Blue;
            WriteTool(FULLSCREEN_X, FULLSCREEN_UP_Y + 20, "                                                                                          ");
            WriteTool(FULLSCREEN_X, FULLSCREEN_UP_Y + 21, "                                                                                          ");
            WriteTool(FULLSCREEN_X, FULLSCREEN_UP_Y + 22, "                                                                                          ");

            Console.ResetColor();

        }
    }

    // 장애물과 아이템 출력
    class Object : GameManager
    {
        // 오브젝트의 최초 생성 위치
        protected const int OBJECT_BOTTOM_X = 105,
                            OBJECT_BOTTOM_Y = 20,
                            PRINT_LEFT = 25;
        // 오브젝트의 x좌표
        public static int object_x = OBJECT_BOTTOM_X;
        protected static int i = 0, j = 0, k = 0;
        // 커비의 공격 유뮤 / 커비와 오브젝트의 접촉 여부 / 커비가 데미지 입음 여부
        public static bool attack_time = false, kirby_Match = false, kirby_hurt = false;

        // 위치를 초기화 해주는 함수
        protected virtual void Move()
        {
            // 오브젝트의 위치를 -4씩 이동시킴
            Object.object_x -= 4;
            // Object의 X좌표가 화면 왼쪽보다 작으면 (화면 왼쪽을 넘어가면)
            if (object_x <= PRINT_LEFT)
            {
                // 스테이지 클리어 여부
                if (!GameManager.stage_1_clear)
                {
                    // x 위치 원위치
                    object_x = OBJECT_BOTTOM_X;
                }
                else
                {
                    // 화면 밖에 위치시키기
                    object_x = OBJECT_BOTTOM_X - 24;
                }
            }
        }

        // 스토리 모드 오브젝트 생성
        public void Make_Object()
        {
            // 각 오브젝트 클래스 인스턴스화
            Thron thron = new Thron();
            Cloud cloud = new Cloud();
            Apple apple = new Apple();

            Random r = new Random();

            // 오브젝트의 x 좌표가 화면 왼쪽보다 낮거나 커비와 오브젝트가 닿았다면 물체 위치 초기화 및 새로운 오브젝트 생성
            if (Object.object_x <= PRINT_LEFT + 4 || kirby_Match)
            {
                // i에 새로운 변수 입력
                i = r.Next();
                kirby_hurt = false;
                kirby_Match = false;
                Object.object_x = PRINT_LEFT;
            }
            else
            {
                if (i % 6 < 3)  // 2/3 확률로 사과 아이템 생성 
                {
                    apple.Move();
                }
                else if (i % 6 == 5)  // 1/6 확률로 구름 장애물 생성
                {
                    cloud.Move();
                }
                else  // 1/6 확률로 가시 장애물 생성
                {
                    thron.Move();
                }
            }


        }

        // 보스 모드 아이템 생성
        public void Make_Object_2()
        {
            Print print = new Print();
            Attack attack = new Attack();
            High_Fire high_Fire = new High_Fire();
            Low_Fire low_Fire = new Low_Fire();
            Random r = new Random();
            if (j == 5) // 어떻게 충분한 시간을 줄지 생각해보기
            {
                attack_time = true;
                WriteTool(50, 10, "쿠파가 지쳤어 지금이 공격할 타이밍이야!");
                j = 0;
            }
            if (!attack_time)
            {
                if (Object.object_x <= PRINT_LEFT + 4 || kirby_Match)
                {
                    i = r.Next();
                    kirby_hurt = false;
                    kirby_Match = false;
                    Object.object_x = PRINT_LEFT;
                    j++;
                }
                if (j != 5)
                {
                    if (i % 3 == 0)
                    {
                        high_Fire.Move();
                    }
                    else
                    {
                        low_Fire.Move();
                    }
                }
            }


        }
        public void Erase_Object()
        {
            for (int i = 8; i < OBJECT_BOTTOM_Y + 3; i++)
            {
                WriteTool(object_x, i, "          ");
            }
        }
        void WriteTool(int x, int y, string s)
        {
            SetCursorPosition(x, y);
            Write(s);
        }
    }
    class Object_Double : GameManager
    {
        protected const int OBJECT_BOTTOM_X = 105,
                            OBJECT_BOTTOM_Y = 20,
                            PRINT_LEFT = 25;
        public static int object_x = OBJECT_BOTTOM_X;
        protected static int i = 0, j = 0, k = 0;
        public static bool attack_time = false, kirby_Match = false, kirby_hurt = false, stop = false;

        protected virtual void Move()
        {
            Object_Double.object_x -= 4;
            if (object_x <= PRINT_LEFT)
            {
                object_x = OBJECT_BOTTOM_X;
            }
        }
        public void Make_Object()
        {
            Random r = new Random();
            Thron_Upgrade thron_Upgrade = new Thron_Upgrade();
            Cloud_Upgrade cloud_Upgrade = new Cloud_Upgrade();

            if (Object_Double.object_x <= PRINT_LEFT + 4 || kirby_Match || Object_Double.object_x == OBJECT_BOTTOM_X)
            {
                stop = true;
                kirby_hurt = false;
                kirby_Match = false;
                Object_Double.object_x = OBJECT_BOTTOM_X;
            }

            if (Object.object_x > 40 && Object.object_x < 70 && Object_Double.object_x == OBJECT_BOTTOM_X)
            {
                i = r.Next(0, 10);
                stop = false;
            }

            if (!stop)
            {
                if (i % 3 == 0)
                {
                    cloud_Upgrade.Move();

                }
                else
                {
                    thron_Upgrade.Move();

                }
            }
        }
        public void Erase_Object()
        {
            for (int i = 8; i < OBJECT_BOTTOM_Y + 3; i++)
            {
                WriteTool(object_x, i, "          ");
            }
        }
        void WriteTool(int x, int y, string s)
        {
            SetCursorPosition(x, y);
            Write(s);
        }
    }
    class Thron : Object
    {
        protected override void Move()
        {
            base.Move();
            Damage();
            Thorn_shape();
        }
        public void Damage()
        {
            // 오브젝트의 x 좌표가 커비의 x좌표 이고, 커비의 y 좌표가 오브젝트의 Y값과 같을 때 : 커비 체력 -=1
            if (object_x <= PRINT_LEFT + 16 && GameManager.kirby_Y == OBJECT_BOTTOM_Y)
            {
                GameManager.kirby_life -= 1;
                Object.kirby_Match = true;
                Object.kirby_hurt = true;
            }
        }
        public void Thorn_shape()
        {
            ForegroundColor = ConsoleColor.Red;
            WriteTool(object_x, OBJECT_BOTTOM_Y, "    ▲");
            WriteTool(object_x, OBJECT_BOTTOM_Y + 1, "  ▲  ▲");
            WriteTool(object_x, OBJECT_BOTTOM_Y + 2, "▲      ▲");
            ForegroundColor = ConsoleColor.White;
            BackgroundColor = ConsoleColor.Red;
            WriteTool(object_x + 4, OBJECT_BOTTOM_Y + 1, "■");
            WriteTool(object_x + 2, OBJECT_BOTTOM_Y + 2, "■■■");
            Console.ResetColor();
        }
        void WriteTool(int x, int y, string s)
        {
            SetCursorPosition(x, y);
            Write(s);
        }
    }
    class Thron_Upgrade : Object_Double
    {
        protected override void Move()
        {
            base.Move();
            Damage();
            Thorn_shape();
        }
        public void Damage()
        {
            if (object_x <= PRINT_LEFT + 16)
            {
                if (GameManager.kirby_Y == OBJECT_BOTTOM_Y)
                {
                    GameManager.kirby_life -= 1;
                    Object_Double.kirby_Match = true;
                    Object_Double.kirby_hurt = true;
                }
            }
            //객체의 x 좌표가 커비의 x좌표 일때 , 커비의 y 좌표가 (지정된 화면 하단) y값이 아닐때 : 커비 체력 -=1
        }
        public void Thorn_shape()
        {
            ForegroundColor = ConsoleColor.Red;
            WriteTool(object_x + 1, OBJECT_BOTTOM_Y, "▲");
            WriteTool(object_x + 7, OBJECT_BOTTOM_Y, "▲");
            WriteTool(object_x, OBJECT_BOTTOM_Y + 1, "▲ ▲");
            WriteTool(object_x + 6, OBJECT_BOTTOM_Y + 1, "▲ ▲");
            WriteTool(object_x - 2, OBJECT_BOTTOM_Y + 2, "▲     ▲    ▲");
            WriteTool(object_x - 2, OBJECT_BOTTOM_Y + 2, "▲     ▲    ▲");
            Console.ResetColor();
        }
        void WriteTool(int x, int y, string s)
        {
            SetCursorPosition(x, y);
            Write(s);
        }
    }
    class Cloud : Object
    {
        new int OBJECT_BOTTOM_Y = 8;
        protected override void Move()
        {
            base.Move();
            Damage();
            Cloud_shape();
        }
        public void Damage()
        {
            if (object_x < PRINT_LEFT + 15 && object_x > PRINT_LEFT)
            {
                if (GameManager.kirby_Y <= OBJECT_BOTTOM_Y + 5)
                {
                    GameManager.kirby_life -= 1;
                    Object.kirby_Match = true;
                    Object.kirby_hurt = true;
                }
            }
            //객체의 x 좌표가 커비의 x좌표 일때 , 커비의 y 좌표가 (지정된 화면 하단) y값이 아닐때 : 커비 체력 -=1
        }
        public void Cloud_shape()
        {
            ForegroundColor = ConsoleColor.White;
            WriteTool(object_x + 2, OBJECT_BOTTOM_Y, "▲▲▲");
            WriteTool(object_x, OBJECT_BOTTOM_Y + 1, "◀      ▶");
            WriteTool(object_x + 2, OBJECT_BOTTOM_Y + 2, "▼▼▼");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(object_x + 2, OBJECT_BOTTOM_Y + 1, " X __X ");
            Console.ResetColor();
        }
        void WriteTool(int x, int y, string s)
        {
            SetCursorPosition(x, y);
            Write(s);
        }
    }
    class Cloud_Upgrade : Object_Double
    {
        new int OBJECT_BOTTOM_Y = 8;
        protected override void Move()
        {
            base.Move();
            Damage();
            Cloud_shape();
        }
        public void Damage()
        {
            if (object_x < PRINT_LEFT + 20 && object_x > PRINT_LEFT)
            {
                if (GameManager.kirby_Y <= OBJECT_BOTTOM_Y + 5)
                {
                    GameManager.kirby_life -= 1;
                    Object_Double.kirby_Match = true;
                    Object_Double.kirby_hurt = true;
                }
            }
            //객체의 x 좌표가 커비의 x좌표 일때 , 커비의 y 좌표가 (지정된 화면 하단) y값이 아닐때 : 커비 체력 -=1
        }
        public void Cloud_shape()
        {
            ForegroundColor = ConsoleColor.White;
            WriteTool(object_x + 2, OBJECT_BOTTOM_Y - 2, "▲▲▲");
            WriteTool(object_x, OBJECT_BOTTOM_Y - 1, "▲      ▲");
            WriteTool(object_x - 2, OBJECT_BOTTOM_Y, "◀          ▶");
            WriteTool(object_x, OBJECT_BOTTOM_Y + 1, "▼      ▼");
            WriteTool(object_x + 2, OBJECT_BOTTOM_Y + 2, "▼▼▼");
            BackgroundColor = ConsoleColor.Red;
            WriteTool(object_x + 2, OBJECT_BOTTOM_Y + 1, " X __X ");
            Console.ResetColor();
        }
        void WriteTool(int x, int y, string s)
        {
            SetCursorPosition(x, y);
            Write(s);
        }
    }
    class Apple : Object
    {
        protected override void Move()
        {
            base.Move();
            Eat();
            Apple_shape();
        }
        public void Eat()
        {
            if (object_x <= PRINT_LEFT + 16)
            {
                if (GameManager.kirby_Y == OBJECT_BOTTOM_Y)
                {
                    GameManager.ate_apple += 1;
                    Object.kirby_Match = true;
                }
            }
            //객체의 x 좌표가 커비의 x좌표 일때 , 커비의 y 좌표가 (지정된 화면 하단) y값이 아닐때 : 커비 체력 -=1
        }
        public void Apple_shape()
        {
            BackgroundColor = ConsoleColor.Green;
            ForegroundColor = ConsoleColor.White;
            WriteTool(object_x + 4, OBJECT_BOTTOM_Y - 1, "♭");
            BackgroundColor = ConsoleColor.White;
            ForegroundColor = ConsoleColor.Yellow;
            WriteTool(object_x + 2, OBJECT_BOTTOM_Y, "■■■");
            WriteTool(object_x, OBJECT_BOTTOM_Y + 1, "■■■■■");
            WriteTool(object_x + 2, OBJECT_BOTTOM_Y + 2, "■■■");
            Console.ResetColor();
        }
        void WriteTool(int x, int y, string s)
        {
            SetCursorPosition(x, y);
            Write(s);
        }

    }
    class High_Fire : Object
    {
        new int OBJECT_BOTTOM_Y = 8;

        protected override void Move()
        {
            base.Move();
            Damage();
            Fire_shape();
        }
        public void Damage()
        {
            if (object_x < PRINT_LEFT + 10 && object_x > PRINT_LEFT)
            {
                if (GameManager.kirby_Y <= OBJECT_BOTTOM_Y + 5)
                {
                    GameManager.kirby_life -= 1;
                    Object.kirby_Match = true;
                    Object.kirby_hurt = true;
                }
            }
            //객체의 x 좌표가 커비의 x좌표 일때 , 커비의 y 좌표가 (지정된 화면 하단) y값이 아닐때 : 커비 체력 -=1
        }
        public void Fire_shape()
        {
            //BackgroundColor = ConsoleColor.DarkRed; //보류
            ForegroundColor = ConsoleColor.DarkRed;
            WriteTool(object_x + 4, OBJECT_BOTTOM_Y, "■▶");
            WriteTool(object_x + 2, OBJECT_BOTTOM_Y + 1, "■■■▶");
            WriteTool(object_x + 4, OBJECT_BOTTOM_Y + 2, "■▶");
            ForegroundColor = ConsoleColor.Red;
            WriteTool(object_x, OBJECT_BOTTOM_Y + 1, "■");
            WriteTool(object_x + 2, OBJECT_BOTTOM_Y + 2, "■");
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.Yellow;
            WriteTool(object_x + 6, OBJECT_BOTTOM_Y, "▶");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteTool(object_x + 8, OBJECT_BOTTOM_Y + 1, "▶");
            WriteTool(object_x + +6, OBJECT_BOTTOM_Y + 2, "▶");
            Console.ResetColor();
        }
        void WriteTool(int x, int y, string s)
        {
            SetCursorPosition(x, y);
            Write(s);
        }
    }
    class Low_Fire : Object
    {
        protected override void Move()
        {
            base.Move();
            Damage();
            Fire_shape();
        }
        public void Damage()
        {
            if (object_x <= PRINT_LEFT + 16)
            {
                if (GameManager.kirby_Y == OBJECT_BOTTOM_Y)
                {
                    GameManager.kirby_life -= 1;
                    Object.kirby_Match = true;
                    Object.kirby_hurt = true;
                }
            }
            //객체의 x 좌표가 커비의 x좌표 일때 , 커비의 y 좌표가 (지정된 화면 하단) y값이 아닐때 : 커비 체력 -=1
        }
        public void Fire_shape()
        {
            //BackgroundColor = ConsoleColor.DarkRed; //보류
            ForegroundColor = ConsoleColor.DarkRed;
            WriteTool(object_x + 4, OBJECT_BOTTOM_Y, "■▶");
            WriteTool(object_x + 2, OBJECT_BOTTOM_Y + 1, "■■■▶");
            WriteTool(object_x + 4, OBJECT_BOTTOM_Y + 2, "■▶");
            ForegroundColor = ConsoleColor.Red;
            WriteTool(object_x, OBJECT_BOTTOM_Y + 1, "■");
            WriteTool(object_x + 2, OBJECT_BOTTOM_Y + 2, "■");
            BackgroundColor = ConsoleColor.Black;
            ForegroundColor = ConsoleColor.Yellow;
            WriteTool(object_x + 6, OBJECT_BOTTOM_Y, "▶");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteTool(object_x + 8, OBJECT_BOTTOM_Y + 1, "▶");
            WriteTool(object_x + +6, OBJECT_BOTTOM_Y + 2, "▶");
            Console.ResetColor();
        }
        void WriteTool(int x, int y, string s)
        {
            SetCursorPosition(x, y);
            Write(s);
        }
    }
    class Attack : GameManager
    {
        public const int ATTACK_BOTTOM_X = 40,
                            ATTACK_BOTTOM_Y = 20,
                             PRINT_RIGHT = 82;
        public static int attack_x = ATTACK_BOTTOM_X;

        public void Attack_Action()
        {
            Move();
            Damage_Koopa();
            Attack_shape();
        }
        protected virtual void Move()
        {
            Attack.attack_x += 5;

            if (attack_x >= PRINT_RIGHT)
            {
                attack_x = ATTACK_BOTTOM_X;
                Print.b_attack = false;
                Object.attack_time = false;
            }
        }

        public void Damage_Koopa()
        {
            Print print = new Print();
            if (attack_x > PRINT_RIGHT - 5)
            {
                GameManager.koopa_life -= 1;
                GameManager.ate_apple -= 1;
                Input input = new Input();
                Thread stun = new Thread(input.Stun_print);
                stun.Start();
                print.Koopa_damage_print();
                stun.Join();
                print.Koopa_print();
            }
        }
        public void Attack_shape()
        {
            Console.ResetColor();
            BackgroundColor = ConsoleColor.Green;
            WriteTool(attack_x + 4, ATTACK_BOTTOM_Y - 1, "♭");
            BackgroundColor = ConsoleColor.White;
            ForegroundColor = ConsoleColor.Yellow;
            WriteTool(attack_x + 2, ATTACK_BOTTOM_Y, "■■■");
            WriteTool(attack_x, ATTACK_BOTTOM_Y + 1, "■■■■■");
            WriteTool(attack_x + 2, ATTACK_BOTTOM_Y + 2, "■■■");
            Console.ResetColor();
        }
        public void Erase_Attack_shape()
        {
            Console.ResetColor();
            WriteTool(attack_x, ATTACK_BOTTOM_Y - 1, "          ");
            WriteTool(attack_x, ATTACK_BOTTOM_Y, "          ");
            WriteTool(attack_x, ATTACK_BOTTOM_Y + 1, "          ");
            WriteTool(attack_x, ATTACK_BOTTOM_Y + 2, "          ");
        }
        void WriteTool(int x, int y, string s)
        {
            SetCursorPosition(x, y);
            Write(s);
        }
    }
    class Program
    {
        enum Menu { 스토리모드, 무한모드, 도전과제, 게임옵션, 게임종료, }
        public const int INTERVAL_Y = 3, MENU_POSITION_X = 66, MENU_POSITION_Y = 42, ARROW_POSION_GAP = 8, LONG_TEXT_X = 30;
        public const string ARROW = "▶";
        public static bool storymode = false;

        static void Menu_print(int arrow_position, int prev_arrow_position)
        {
            Print print = new Print(); // 시작화면 
            print.Nintendo_Low_Screen_Print();

            BackgroundColor = ConsoleColor.White;
            ForegroundColor = ConsoleColor.Black;
            WriteTool(MENU_POSITION_X, MENU_POSITION_Y + INTERVAL_Y * (int)Menu.스토리모드, "스토리모드");
            WriteTool(MENU_POSITION_X, MENU_POSITION_Y + INTERVAL_Y * (int)Menu.무한모드, "무한모드");
            WriteTool(MENU_POSITION_X, MENU_POSITION_Y + INTERVAL_Y * (int)Menu.도전과제, "도전과제");
            WriteTool(MENU_POSITION_X, MENU_POSITION_Y + INTERVAL_Y * (int)Menu.게임옵션, "게임옵션");
            WriteTool(MENU_POSITION_X, MENU_POSITION_Y + INTERVAL_Y * (int)Menu.게임종료, "게임종료");
            WriteTool(MENU_POSITION_X - ARROW_POSION_GAP, MENU_POSITION_Y + INTERVAL_Y * arrow_position, ARROW);
            WriteTool(MENU_POSITION_X - ARROW_POSION_GAP, MENU_POSITION_Y + INTERVAL_Y * prev_arrow_position, "  ");
            WriteTool(MENU_POSITION_X - 14, MENU_POSITION_Y + INTERVAL_Y - 6, "방향키로 커서를 움직이고 엔터를 눌러주세요!");
            ForegroundColor = ConsoleColor.Blue;
            if (arrow_position == (int)Menu.스토리모드) { WriteTool(MENU_POSITION_X + 11, MENU_POSITION_Y + INTERVAL_Y * (int)Menu.스토리모드, ": 커비와 함께 모험을 떠나자"); }
            if (arrow_position == (int)Menu.도전과제) { WriteTool(MENU_POSITION_X + 10, MENU_POSITION_Y + INTERVAL_Y * (int)Menu.도전과제, ": 획득한 일러스트들을 구경하자"); }
            ForegroundColor = ConsoleColor.Red;
            if (arrow_position == (int)Menu.무한모드) { WriteTool(MENU_POSITION_X + 10, MENU_POSITION_Y + INTERVAL_Y * (int)Menu.무한모드, ": 스토리모드 클리어시 플레이 가능합니다"); }
            Console.ResetColor();
        }
        static void Nintendo_ON()
        {
            Print print = new Print(); // 시작화면 
            for (int j = 0; j < 3; j++)
            {
                ForegroundColor = ConsoleColor.DarkGreen;
                WriteTool(134, 31, "┃┃┃");
                ForegroundColor = ConsoleColor.White;
                BackgroundColor = ConsoleColor.Black;
                for (int i = 4; i < 27; i++)
                {
                    WriteTool(26, i, "                                                                                          ");
                }
                for (int i = 36; i < 57; i++)
                {
                    WriteTool(26, i, "                                                                                          ");
                }
                Thread.Sleep(300);
                print.Nintendo_print();
                Thread.Sleep(200);
            }
        }
        static void Main(string[] args)
        {
            SetWindowSize(144, 62);
            SetBufferSize(144, 62);
            CursorVisible = false;
            Print print = new Print(); // 시작화면 
            print.Nintendo_print();
            ForegroundColor = ConsoleColor.Black;
            BackgroundColor = ConsoleColor.White;
            WriteTool(52, 14, "창고에서 닌텐도를 찾았다. 한번 켜볼까?");
            WriteTool(52, 44, "닌텐도를 키려면 ENTER를 누르세요.");
            ReadKey();
            Nintendo_ON();

            while (true) // 키보드 입력 받고 게임 시작
            {
                print.Nintendo_High_Screen_Print();
                int arrow_position = 0, prev_arrow_position = -1;

                while (true) //키 입력
                {
                    //print.Nintendo_High_Screen_Print();
                    Menu_print(arrow_position, prev_arrow_position);
                    prev_arrow_position = arrow_position;

                    ConsoleKey key = ReadKey().Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            {
                                if (arrow_position != (int)Menu.스토리모드)
                                {
                                    arrow_position--;
                                }
                                else arrow_position = (int)Menu.게임종료;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            {
                                if (arrow_position != (int)Menu.게임종료)
                                {
                                    arrow_position++;
                                }
                                else arrow_position = (int)Menu.스토리모드;
                            }
                            break;
                        case ConsoleKey.Enter:
                            {
                                if (arrow_position == (int)Menu.스토리모드)
                                {
                                    GameManager.ingame_start = true;
                                }
                                if (arrow_position == (int)Menu.무한모드)
                                {

                                }
                                if (arrow_position == (int)Menu.도전과제)
                                {

                                }
                                if (arrow_position == (int)Menu.게임옵션)
                                {

                                }
                                if (arrow_position == (int)Menu.게임종료)
                                {
                                    Console.ResetColor();
                                    Clear();
                                    return;
                                }
                            }
                            break;
                    }
                    if (GameManager.ingame_start)
                    {
                        break;
                    }
                }
                if (GameManager.ingame_start)
                {
                    GameManager gameManager = new GameManager();
                    Thread thread = new Thread(Input_Thread);
                    thread.Start();

                    gameManager.Reset();
                    gameManager.GameStart();
                    gameManager.GameEnding();
                    if (GameManager.gameover)
                    {
                        prev_arrow_position = -1;
                        GameManager.ingame_start = false;
                    }
                }
            }
        }
        public static void Input_Thread()
        {
            Input input = new Input();
            input.Input_key();
        }
        static void WriteTool(int x, int y, string s)
        {
            SetCursorPosition(x, y);
            Write(s);
        }

    }
}
