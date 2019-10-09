﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using GmodNET.API;
using System.Runtime.InteropServices;
using static GmodNET.LuaInterop;

namespace GmodNET
{
    internal class Lua : ILua
    {
        IntPtr ptr;
        internal Lua(IntPtr ptr)
        {
            this.ptr = ptr;
        }

        public int Top()
        {
            return top(ptr);
        }

        public void Push(int iStackPos)
        {
            push(ptr, iStackPos);
        }

        public void Pop(int IAmt)
        {
            pop(ptr, IAmt);
        }

        public void GetField(int iStackPos, in string key)
        {
            byte[] buff = Encoding.UTF8.GetBytes(key);
            unsafe
            {
                fixed(byte * tmp_ptr = &buff[0])
                {
                    get_field(ptr, iStackPos, (IntPtr)tmp_ptr);
                }
            }
        }

        public void SetField(int iStackPos, in string key)
        {
            byte[] buff = Encoding.UTF8.GetBytes(key);
            unsafe
            {
                fixed(byte * tmp_ptr = &buff[0])
                {
                    set_field(ptr, iStackPos, (IntPtr)tmp_ptr);
                }
            }
        }

        public void CreateTable()
        {
            create_table(ptr);
        }

        public void SetMetaTable(int iStacPos)
        {
            set_metatable(ptr, iStacPos);
        }

        public bool GetMetaTable(int iStackPos)
        {
            int tmp = get_metatable(ptr, iStackPos);

            if(tmp == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Call(int iArgs, int iResults)
        {
            call(ptr, iArgs, iResults);
        }

        public int PCall(int IArgs, int IResults, int ErrorFunc)
        {
            return pcall(ptr, IArgs, IResults, ErrorFunc);
        }

        public bool Equal(int iA, int iB)
        {
            int tmp = equal(ptr, iA, iB);
            
            if(tmp == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool RawEqual(int iA, int iB)
        {
            int tmp = raw_equal(ptr, iA, iB);

            if(tmp == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Insert(int iStackPos)
        {
            insert(ptr, iStackPos);
        }

        public void Remove(int iStackPos)
        {
            remove(ptr, iStackPos);
        }

        public int Next(int iStackPos)
        {
            return next(ptr, iStackPos);
        }

        public void ThrowError(in string error_message)
        {
            byte[] buff = Encoding.UTF8.GetBytes(error_message);
            unsafe
            {
                fixed(byte * tmp_ptr = &buff[0])
                {
                    throw_error(ptr, (IntPtr)tmp_ptr);
                }
            }
        }

        public void CheckType(int iStackPos, int IType)
        {
            check_type(ptr, iStackPos, IType);
        }

        public void ArgError(int iArgNum, in string error_message)
        {
            byte[] buff = Encoding.UTF8.GetBytes(error_message);
            unsafe
            {
                fixed(byte * tmp_ptr = &buff[0])
                {
                    arg_error(ptr, iArgNum, (IntPtr)tmp_ptr);
                }
            }
        }

        public string GetString(int iStackPos)
        {
            uint len = 0;
            unsafe
            {
                uint * tmp_ptr = &len;
                IntPtr c_str = get_string(ptr, iStackPos, (IntPtr)tmp_ptr);
                if(c_str == IntPtr.Zero)
                {
                    return string.Empty;
                }
                return Encoding.UTF8.GetString((byte*)c_str, (int)len);
            }
        }

        public double GetNumber(int iStackPos)
        {
            return get_number(ptr, iStackPos);
        }

        public bool GetBool(int iStackPos)
        {
            int tmp = get_bool(ptr, iStackPos);

            if(tmp == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public IntPtr GetCFunction(int iStackPos)
        {
            return get_c_function(ptr, iStackPos);
        }

        public void PushNil()
        {
            push_nil(ptr);
        }

        public void PushString(in string str)
        {
            byte[] buff = Encoding.UTF8.GetBytes(str);

            unsafe
            {
                fixed(byte * tmp_ptr = &buff[0])
                {
                    push_string(ptr, (IntPtr)tmp_ptr, (uint)buff.Length);
                }
            }
        }

        public void PushNumber(double val)
        {
            push_number(ptr, val);
        }

        public void PushBool(bool val)
        {
            int tmp;

            if(val)
            {
                tmp = 1;
            }
            else
            {
                tmp = 0;
            }

            push_bool(ptr, tmp);
        }

        public void PushCFunction(CFuncManagedDelegate managed_function)
        {
            IntPtr marshaled_function = Marshal.GetFunctionPointerForDelegate<CFuncManagedDelegate>(managed_function);

            push_c_function(ptr, marshaled_function);
        }

        public void PushCFunction(IntPtr native_func_ptr)
        {
            push_c_function(ptr, native_func_ptr);
        }

        public void PushCClosure(IntPtr native_func_ptr, int iVars)
        {
            push_c_closure(ptr, native_func_ptr, iVars);
        }

        public int ReferenceCreate()
        {
            return reference_create(ptr);
        }

        public void ReferenceFree(int reference)
        {
            reference_free(ptr, reference);
        }

        public void ReferencePush(int reference)
        {
            reference_push(ptr, reference);
        }

        public void PushSpecial(SPECIAL_TABLES table)
        {
            push_special(ptr, (int)table);
        }

        public bool IsType(int iStackPos, int iType)
        {
            int tmp = is_type(ptr, iStackPos, iType);

            if(tmp == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int GetType(int iStackPos)
        {
            return get_type(ptr, iStackPos);
        }

        public string GetTypeName(int iType)
        {
            int len = 0;
            
            unsafe
            {
                int * len_ptr = &len;
                
                IntPtr c_str = get_type_name(ptr, iType, (IntPtr)len_ptr);

                return Encoding.UTF8.GetString((byte*)c_str, len);
            }
        }

        public int ObjLen(int iStackPos)
        {
            return obj_len(ptr, iStackPos);
        }

        public Vector3 GetAngle(int iStackPos)
        {
            Span<float> components = stackalloc float[3];
            unsafe
            {
                fixed(float * tmp_ptr = &components[0])
                {
                    get_angle(ptr, (IntPtr)tmp_ptr, iStackPos);
                }
            }

            return new Vector3(components[0], components[1], components[2]);
        }

        public Vector3 GetVector(int iStackPos)
        {
            Span<float> components = stackalloc float[3];
            unsafe
            {
                fixed(float * tmp_ptr = &components[0])
                {
                    get_vector(ptr, (IntPtr)tmp_ptr, iStackPos);
                }
            }

            return new Vector3(components[0], components[1], components[2]);
        }

        public void PushAngle(Vector3 ang)
        {
            push_angle(ptr, ang.X, ang.Y, ang.Z);
        }

        public void PushVector(Vector3 vec)
        {
            push_vector(ptr, vec.X, vec.Y, vec.Z);
        }

        public void SetState(IntPtr lua_state)
        {
            set_state(ptr, lua_state);
        }

        public int CreateMetaTable(in string name)
        {
            byte[] buff = Encoding.UTF8.GetBytes(name);

            unsafe
            {
                fixed(byte * tmp_ptr = &buff[0])
                {
                    return create_metatable(ptr, (IntPtr)tmp_ptr);
                }
            }
        }

        public bool PushMetaTable(int iType)
        {
            int tmp = push_metatable(ptr, iType);

            if(tmp == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void PushUserType(IntPtr data_pointer, int iType)
        {
            push_user_type(ptr, data_pointer, iType);
        }

        public void SetUserType(int iStackPos, IntPtr data_pointer)
        {
            set_user_type(ptr, iStackPos, data_pointer);
        }

        public IntPtr GetInternalPointer()
        {
            return ptr;
        }
    }
}