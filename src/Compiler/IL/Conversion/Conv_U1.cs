﻿/*
* PROJECT:          Atomix Development
* LICENSE:          Copyright (C) Atomix Development, Inc - All Rights Reserved
*                   Unauthorized copying of this file, via any medium is
*                   strictly prohibited Proprietary and confidential.
* PURPOSE:          Conv_U MSIL
* PROGRAMMERS:      Aman Priyadarshi (aman.eureka@gmail.com)
*/

using System;
using System.Reflection;

using Atomix.Assembler;
using Atomix.Assembler.x86;
using Atomix.CompilerExt;
using Core = Atomix.Assembler.AssemblyHelper;

namespace Atomix.IL
{
    [ILOp(ILCode.Conv_U1)]
    public class Conv_U1 : MSIL
    {
        public Conv_U1(Compiler Cmp)
            : base("convu1", Cmp) { }

        public override void Execute(ILOpCode instr, MethodBase aMethod)
        {
            var xSource = Core.vStack.Pop();

            //Convert to int8, pushing int32 on stack.
            //Mean Just Convert Last 1 byte to Int32
            switch (ILCompiler.CPUArchitecture)
            {
                #region _x86_
                case CPUArch.x86:
                    {
                        switch (xSource.Size)
                        {
                            case 1:
                            case 2:
                            case 4:
                                {
                                    if (xSource.IsFloat)
                                    {
                                        throw new Exception("Conv_U1 -> Size 4 : Float");
                                    }
                                    else
                                    {
                                        //Just Pop into EAX, zero extend and push it.
                                        Core.AssemblerCode.Add(new Pop { DestinationReg = Registers.EAX });
                                        Core.AssemblerCode.Add(new Movzx { DestinationReg = Registers.EAX, SourceReg = Registers.AL, Size = 8 });
                                        Core.AssemblerCode.Add(new Push { DestinationReg = Registers.EAX });
                                    }
                                }
                                break;
                            case 8:
                                {
                                    if (xSource.IsFloat)
                                    {
                                        throw new Exception("Conv_U1 -> Size 8 : Float");
                                    }
                                    else
                                    {
                                        //Just Erase The 8 byte as we want only first 4 byte manily 1 byte of it.
                                        Core.AssemblerCode.Add(new Pop { DestinationReg = Registers.EAX });
                                        Core.AssemblerCode.Add(new Add { DestinationReg = Registers.ESP, SourceRef = "0x4" });
                                        Core.AssemblerCode.Add(new Movzx { DestinationReg = Registers.EAX, SourceReg = Registers.AL, Size = 8 });
                                        Core.AssemblerCode.Add(new Push { DestinationReg = Registers.EAX });
                                    }
                                }
                                break;
                            default:
                                throw new Exception("Not Yet Implemented Conv_U1 : Size" + xSource.Size);
                        }
                    }
                    break;
                #endregion
                #region _x64_
                case CPUArch.x64:
                    {

                    }
                    break;
                #endregion
                #region _ARM_
                case CPUArch.ARM:
                    {

                    }
                    break;
                #endregion
            }

            Core.vStack.Push(4, typeof(uint));
        }
    }
}
