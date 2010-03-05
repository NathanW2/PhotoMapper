using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace PhotoMapper
{
    [TypeLibType(4096)]
    [InterfaceType(2)]
    [Guid("1D42EC63-7B28-11CE-B83D-00AA002C4F58")]
    public interface DMapInfo
    {
        [DispId(1610678272)]
        object Application { get; }
        [DispId(1610678275)]
        string FullName { get; }
        [DispId(1610678279)]
        int LastErrorCode { get; set; }
        [DispId(1610678281)]
        string LastErrorMessage { get; }
        [DispId(1610678282)]
        object MBApplications { get; }
        [DispId(1610678290)]
        object MIMapGen { get; }
        [DispId(0)]
        string Name { get; }
        [DispId(1610678273)]
        object Parent { get; }
        [DispId(1610678289)]
        int ProductLevel { get; }
        [DispId(1610678276)]
        string Version { get; }
        [DispId(1610678277)]
        bool Visible { get; set; }

        [DispId(1610678287)]
        object DataObject(int windowID);
        [DispId(1610678283)]
        void Do(string command);
        [DispId(1610678284)]
        string Eval(string expression);
        [DispId(1610743808)]
        void RegisterCallback(object callbackobject);
        [DispId(1610678285)]
        void RunCommand(string command);
        [DispId(1610678286)]
        void RunMenuCommand(int id);
        [DispId(1610678288)]
        void SetCallback(object callbackobject);
        [DispId(1610743809)]
        void SetCallbackEvents(object callbackobject, int eventFlags);
        [DispId(1610743810)]
        void UnregisterCallback(object callbackobject);
    }
}
