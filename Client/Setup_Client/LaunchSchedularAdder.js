var checkboxChecked = true;
var checkboxText = "작업 스케줄러 추가 작업 시작";
var filename = "SchedularAdder.exe";

// Installer 에서 구동을 위한 일정한 값들


var msiOpenDatabaseModeTransact = 1;

var msiViewModifyInsert         = 1;
var msiViewModifyUpdate         = 2;
var msiViewModifyAssign         = 3;
var msiViewModifyReplace        = 4;
var msiViewModifyDelete         = 6;

// WScript : 윈도우 스크립트, js 파일 포함 가능, 즉 해당 파일을 말함
if(WScript.Arguments.Length != 1){
    WScript.StdErr.WriteLine(WScript.ScriptName + " file");
    WScript.Quit(1);
}

var filespec = WScript.Arguments(0); 
WScript.StdOut.WriteLine(filespec);
var installer = WScript.CreateObject("WindowsInstaller.Installer");
var database = installer.OpenDatabase(filespec, msiOpenDatabaseModeTransact); 

var sql;
var view;
var record;


try
{
    var fileId = FindFileIdentifier(database, filename);
    if (!fileId)
        throw "Unable to find '" + filename + "' in File table";


    WScript.Echo("Updating the Control table...");
    // Modify the Control_Next of BannerBmp control to point to the new CheckBox
    sql = "SELECT `Dialog_`, `Control`, `Type`, `X`, `Y`, `Width`, `Height`, `Attributes`, `Property`, `Text`, `Control_Next`, `Help` FROM `Control` WHERE `Dialog_`='FinishedForm' AND `Control`='BannerBmp'";
    view = database.OpenView(sql);
    view.Execute();
    record = view.Fetch();
    record.StringData(11) = "CheckboxLaunch";
    view.Modify(msiViewModifyReplace, record);
    view.Close();

    // Resize the BodyText and BodyTextRemove controls to be reasonable
    sql = "SELECT `Dialog_`, `Control`, `Type`, `X`, `Y`, `Width`, `Height`, `Attributes`, `Property`, `Text`, `Control_Next`, `Help` FROM `Control` WHERE `Dialog_`='FinishedForm' AND `Control`='BodyTextRemove'";
    view = database.OpenView(sql);
    view.Execute();
    record = view.Fetch();
    record.IntegerData(7) = 33;
    view.Modify(msiViewModifyReplace, record);
    view.Close();

    sql = "SELECT `Dialog_`, `Control`, `Type`, `X`, `Y`, `Width`, `Height`, `Attributes`, `Property`, `Text`, `Control_Next`, `Help` FROM `Control` WHERE `Dialog_`='FinishedForm' AND `Control`='BodyText'";
    view = database.OpenView(sql);
    view.Execute();
    record = view.Fetch();
    record.IntegerData(7) = 33;
    view.Modify(msiViewModifyReplace, record);
    view.Close();

    // Insert the new CheckBox control
    // Attributes : 2 = 숨김
    sql = "INSERT INTO `Control` (`Dialog_`, `Control`, `Type`, `X`, `Y`, `Width`, `Height`, `Attributes`, `Property`, `Text`, `Control_Next`, `Help`) VALUES ('FinishedForm', 'CheckboxLaunch', 'CheckBox', '18', '117', '343', '12', '2', 'LAUNCHAPP', '{\\VSI_MS_Sans_Serif13.0_0_0}" + checkboxText + "', 'Line1', '|')";
    view = database.OpenView(sql);
    view.Execute();
    view.Close();

    


    WScript.Echo("Updating the ControlEvent table...");
    // Modify the Order of the EndDialog event of the FinishedForm to 1
    sql = "SELECT `Dialog_`, `Control_`, `Event`, `Argument`, `Condition`, `Ordering` FROM `ControlEvent` WHERE `Dialog_`='FinishedForm' AND `Event`='EndDialog'";
    view = database.OpenView(sql);
    view.Execute();
    record = view.Fetch();
    record.IntegerData(6) = 1;
    view.Modify(msiViewModifyReplace, record);
    view.Close();

    // Insert the Event to launch the application
    sql = "INSERT INTO `ControlEvent` (`Dialog_`, `Control_`, `Event`, `Argument`, `Condition`, `Ordering`) VALUES ('FinishedForm', 'CloseButton', 'DoAction', 'VSDCA_Launch', 'LAUNCHAPP=1', '0')";
    view = database.OpenView(sql);
    view.Execute();
    view.Close();



    WScript.Echo("Updating the CustomAction table...");
    // Insert the custom action to launch the application when finished
    sql = "INSERT INTO `CustomAction` (`Action`, `Type`, `Source`, `Target`) VALUES ('VSDCA_Launch', '210', '" + fileId + "', '')";
    view = database.OpenView(sql);
    view.Execute();
    view.Close();



    if (checkboxChecked)
    {
        WScript.Echo("Updating the Property table...");
        // Set the default value of the CheckBox
        sql = "INSERT INTO `Property` (`Property`, `Value`) VALUES ('LAUNCHAPP', '1')";
        view = database.OpenView(sql);
        view.Execute();
        view.Close();
    }



    database.Commit();
}
catch(e)
{
    WScript.StdErr.WriteLine(e);
    WScript.Quit(1);
}



function FindFileIdentifier(database, fileName)
{
    var sql
    var view
    var record

    // First, try to find the exact file name
    sql = "SELECT `File` FROM `File` WHERE `FileName`='" + fileName + "'";
    view = database.OpenView(sql);
    view.Execute();
    record = view.Fetch();
    if (record)
    {
        var value = record.StringData(1);
        view.Close();
        return value;
    }
    view.Close();

    // The file may be in SFN|LFN format.  Look for a filename in this case next
    sql = "SELECT `File`, `FileName` FROM `File`";
    view = database.OpenView(sql);
    view.Execute();
    record = view.Fetch();
    while (record)
    {
        if (StringEndsWith(record.StringData(2), "|" + fileName))
        {
            var value = record.StringData(1);
            view.Close();
            return value;
        }

        record = view.Fetch();
    }
    view.Close();
    
}

function StringEndsWith(str, value)
{
    if (str.length < value.length)
        return false;

    return (str.indexOf(value, str.length - value.length) != -1);
}