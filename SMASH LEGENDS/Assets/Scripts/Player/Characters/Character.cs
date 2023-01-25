using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace Junpyo
{
    public enum CHARACTERNAME { CHEPESYU, GANGNIM, PENUKUE, TRUELOVE, DUSEONIN, PATAL }
    [Serializable]
    public struct PlayerInformation
    {
        //캐릭터이름
        public CHARACTERNAME Name;

        //HP
        public float HP_Max;
        public float Cur_HP;

        //특수스킬 
        public float Skill_Time; //궁극기 충전 채워야하는 양
        public bool SkillOn;
        public bool SkillImotal;
        public bool IsSkillUplayer;

        //궁극기
        public float UltGage_Max;
        public float Cur_UltGage;
        public bool UltimateOn;
        public bool UtimatePrepare;
        public bool UltimateImotal;

        //취소가능한 스킬
        public bool SkillCansle;
        public bool JumpSkillCansle;

        //이동
        public float RunSpeed;
        public float CurRunSpeed;

        //점프
        public float JumpDistance;
        public float CurJumpDistance;
        public float JumpPower;
        public bool IsAttackUplayer;
        public bool JumpSkillPrepare;

        //공격 Effect
        public PlayerSkill Skills;

        //상태 이상
        public bool Fear;
        public bool Curce;

        //매달리기 값
        public float HangSlope;
        public float HangStrigh;
        public float Hang_Y;

        //공격 시 슈퍼아머인지 확인하기 위한 변수
        public bool IsImotal_Skill;
        public bool IsImotal_Ultimate;


        public void Initialization(CHARACTERNAME name)
        {
            //CHARACTERNAME에 따라 초기화를 다르게 설정
            switch (name)
            {
                case CHARACTERNAME.CHEPESYU:
                    {
                        ChePeSyu();
                    }
                    break;

                case CHARACTERNAME.GANGNIM:
                    {
                        Gangnim();
                    }
                    break;

                case CHARACTERNAME.PENUKUE:
                    {
                        PenuKue();
                    }
                    break;

                case CHARACTERNAME.TRUELOVE:
                    {
                        TrueLove();
                    }
                    break;

                case CHARACTERNAME.DUSEONIN:
                    {
                        Duseonin();
                    }
                    break;

                case CHARACTERNAME.PATAL:
                    {
                        Patal();
                    }
                    break;
                default:
                    break;
            }

        }

        public void ChePeSyu()
        {
            Name = CHARACTERNAME.CHEPESYU;

            HP_Max = 3700;
            Cur_HP = 3700;

            Skill_Time = 5;
            SkillOn = true;
            JumpSkillPrepare = false;

            IsSkillUplayer = true;
            IsAttackUplayer = true;

            UltGage_Max = 80;
            Cur_UltGage = 0;
            UltimateOn = false;
            UtimatePrepare = true;

            SkillCansle = false;
            JumpSkillCansle = false;

            CurRunSpeed = 4.0f;
            RunSpeed = CurRunSpeed;

            CurJumpDistance = 2.5f;
            JumpDistance = CurJumpDistance;
            JumpPower = 400.0f;

            Skills = new ChePeSyuSkill();

            Fear = false;
            Curce = false;

            HangSlope = 0.25f;
            HangStrigh = 0.2f;
            Hang_Y = -1.2f;
        }

        public void PenuKue()
        {
            Name = CHARACTERNAME.PENUKUE;

            HP_Max = 3000;
            Cur_HP = 3000;

            Skill_Time = 8;
            SkillOn = true;
            JumpSkillPrepare = false;

            IsSkillUplayer = false;
            IsAttackUplayer = true;

            UltGage_Max = 100;
            Cur_UltGage = 0;
            UltimateOn = false;
            UtimatePrepare = false;

            SkillCansle = true;
            JumpSkillCansle = true;

            CurRunSpeed = 4.0f;
            RunSpeed = CurRunSpeed;

            CurJumpDistance = 2.5f;
            JumpDistance = CurJumpDistance;
            JumpPower = 400.0f;

            Skills = new ChePeSyuSkill();

            Fear = false;
            Curce = false;

            HangSlope = 0.25f;
            HangStrigh = 0.2f;
            Hang_Y = -1.58f;
        }

        public void Gangnim()
        {
            Name = CHARACTERNAME.GANGNIM;

            HP_Max = 4500;
            Cur_HP = 4500;

            Skill_Time = 5;
            SkillOn = true;
            JumpSkillPrepare = true;

            IsSkillUplayer = false;
            IsAttackUplayer = false;

            UltGage_Max = 100;
            Cur_UltGage = 0;
            UltimateOn = false;
            UtimatePrepare = false;

            SkillCansle = true;
            JumpSkillCansle = false;

            CurRunSpeed = 4.0f;
            RunSpeed = CurRunSpeed;

            CurJumpDistance = 2.5f;
            JumpDistance = CurJumpDistance;
            JumpPower = 400.0f;

            Skills = new ChePeSyuSkill();

            Fear = false;
            Curce = false;

            HangSlope = 0.5f;
            HangStrigh = 0f;
            Hang_Y = -1.4f;
        }

        public void Duseonin()
        {
            Name = CHARACTERNAME.DUSEONIN;

            HP_Max = 3000;
            Cur_HP = 3000;

            Skill_Time = 10;
            SkillOn = true;
            JumpSkillPrepare = false;

            IsSkillUplayer = false;
            IsAttackUplayer = true;

            UltGage_Max = 110;
            Cur_UltGage = 0;
            UltimateOn = false;
            UtimatePrepare = false;

            SkillCansle = true;
            JumpSkillCansle = true;

            CurRunSpeed = 4.0f;
            RunSpeed = CurRunSpeed;

            CurJumpDistance = 2.5f;
            JumpDistance = CurJumpDistance;
            JumpPower = 400.0f;

            Skills = new ChePeSyuSkill();

            Fear = false;
            Curce = false;

            HangSlope = 0.5f;
            HangStrigh = 0f;
            Hang_Y = -1.58f;
        }

        public void TrueLove()
        {
            Name = CHARACTERNAME.TRUELOVE;

            HP_Max = 3700;
            Cur_HP = 3700;

            Skill_Time = 8;
            SkillOn = true;
            JumpSkillPrepare = false;

            IsSkillUplayer = false;
            IsAttackUplayer = false;

            UltGage_Max = 120;
            Cur_UltGage = 0;
            UltimateOn = false;
            UtimatePrepare = false;

            SkillCansle = false;
            JumpSkillCansle = false;

            CurRunSpeed = 4.0f;
            RunSpeed = CurRunSpeed;

            CurJumpDistance = 2.5f;
            JumpDistance = CurJumpDistance;
            JumpPower = 400.0f;

            Skills = new ChePeSyuSkill();

            Fear = false;
            Curce = false;

            HangSlope = 0.5f;
            HangStrigh = 0.0f;
            Hang_Y = -1.6f;

        }

        public void Patal()
        {
            Name = CHARACTERNAME.PATAL;

            HP_Max = 5400;
            Cur_HP = 5400;

            Skill_Time = 5;
            SkillOn = true;
            JumpSkillPrepare = false;

            IsSkillUplayer = true;
            IsAttackUplayer = false;

            UltGage_Max = 70;
            Cur_UltGage = 0;
            UltimateOn = false;
            UtimatePrepare = false;

            SkillCansle = true;
            JumpSkillCansle = true;

            CurRunSpeed = 4.0f;
            RunSpeed = CurRunSpeed;

            CurJumpDistance = 2.5f;
            JumpDistance = CurJumpDistance;
            JumpPower = 400.0f;

            Skills = new ChePeSyuSkill();

            Fear = false;
            Curce = false;

            HangSlope = 0.2f;
            HangStrigh = 0.0f;
            Hang_Y = -1.77f;

        }
    }
}
