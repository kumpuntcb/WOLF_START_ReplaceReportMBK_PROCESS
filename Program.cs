using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using WolfApprove.Model;
using WolfApprove.Model.CustomClass;
using WolfApprove.Model.Extension;

namespace WOLF_START_ReplaceReportMBK_PROCESS
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Program));

        private static string logfile
        {
            get
            {
                string _logfile = ConfigurationManager.AppSettings["logfile"];
                if (!string.IsNullOrEmpty(_logfile))
                {
                    return _logfile;
                   
                }
                return "C:\\Users\\thearaphat\\source\\jobrun\\WOLF_START_ReplaceReportMBK_PROCESS\\WOLF_START_ReplaceReportMBK_PROCESS\\Log\\";
            }
        }
        private static string dbconnectstring
        {
            get
            {
                var dbconnectstring = ConfigurationManager.AppSettings["dbconnectstring"];
                if (!string.IsNullOrEmpty(dbconnectstring))
                {
                    return dbconnectstring;
                }
                return "initial catalog=WolfApproveCore.MBKISO;persist security info=True;user id=sa;password=pass@word1;Connection Timeout=200;";
            }
        }

        static void Main(string[] args)
        {
            StartReplaceReport();


        }
        public static void WriteLogFile(String iText)
        {
            String LogFilePath = String.Format("{0}{1}_Log.txt", logfile, DateTime.Now.ToString("yyyyMMdd"));

            try
            {
                using (System.IO.StreamWriter outfile = new System.IO.StreamWriter(LogFilePath, true))
                {
                    System.Text.StringBuilder sbLog = new System.Text.StringBuilder();

                    //sbLog.AppendLine(String.Empty);
                    //sbLog.AppendLine(String.Format("--------------- Start Time ({0}) ---------------", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));

                    String[] ListText = iText.Split('|').ToArray();

                    foreach (String s in ListText)
                    {
                        sbLog.AppendLine(s);
                    }

                    //sbLog.AppendLine(String.Format("--------------- End Time ({0})   ---------------", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                    //sbLog.AppendLine(String.Empty);

                    outfile.WriteLine(string.Format("{0} - {1}", DateTime.Now.ToString("hh:mm:ss tt"), sbLog.ToString()));
                }
            }
            catch (Exception ex) { }
        }
        public static void StartReplaceReport()
        {
            DataClasses1DataContext db = new DataClasses1DataContext(dbconnectstring);
            try
            {
             
                
                if (db.Connection.State == ConnectionState.Open)
                {
                    db.Connection.Close();
                    db.Connection.Open();
                }
                db.Connection.Open();
                db.CommandTimeout = 0;
                WriteLogFile("Start Job");
                WriteLogFile(db.Connection.State.ToString());
                WriteLogFile(dbconnectstring.ToString());
                List<MSTEmployee> getEmployee = db.MSTEmployees.Where(z => z.IsActive == false).ToList();
                List<MSTEmployee> getEmployee2 = db.MSTEmployees.ToList();
                WriteLogFile(getEmployee2.ToString());
                List<ViewEmployee> getviewemp = db.ViewEmployees.ToList();
                WriteLogFile(getviewemp.ToString());
                List<int?> getIsactive = getEmployee.Select(z => (int?)z.EmployeeId).ToList();
                List<TRNMemo> getMemoAll = db.TRNMemos.Where(z => z.StatusName == "Wait for Approve").ToList();
                WriteLogFile("getMemoAll" + getMemoAll);
                List<TRNMemo> getMemo = db.TRNMemos.Where(z => z.StatusName == "Wait for Approve").ToList();
                WriteLogFile("getMemoWaitApp " + getMemo.Count);
                getMemo = (from a in getMemo join b in getIsactive on a.PersonWaitingId equals b select new TRNMemo { MemoId = a.MemoId, PersonWaitingId = a.PersonWaitingId, PersonWaiting = a.PersonWaiting }).ToList();
                WriteLogFile("getMemo " + getMemo.Count);
                int i = 0;
                foreach (var getdata in getMemo)
                {
                    i++;
                    Console.WriteLine("getMemoCount " + getMemo.Count());
                    Console.WriteLine("getdataMemoid " + getdata.MemoId +" "+ i);
                    WriteLogFile("getMemoCount " + getMemo.Count());
                    WriteLogFile(getdata.MemoId + i.ToString());
                    bool hasNewEmployees = true;
                    var filteredData = getEmployee2.FindAll(z => z.EmployeeId == getdata.PersonWaitingId).ToList();
                    int j = 0;
                    while (hasNewEmployees)
                    {
                        j++;
                        List<MSTEmployee> additionalEmployees = new List<MSTEmployee>();
                        foreach (var getnextemp in filteredData)
                        {
                            WriteLogFile("filteredData" + filteredData.Count);
                            // Console.WriteLine("getnextemp " + i + getnextemp.EmployeeId.ToString());
                            Console.WriteLine("Employeeid = " + getnextemp.EmployeeId.ToString() + " Isative = " + getnextemp.IsActive.ToString());
                            WriteLogFile("Employeeid = " + getnextemp.EmployeeId.ToString() + " Isative = " + getnextemp.IsActive.ToString());
                            WriteLogFile("getnextemp " + i +" "+ getnextemp.EmployeeId.ToString());
                            WriteLogFile(getdata.MemoId +" "+ i.ToString());
                            var getreemp = getEmployee2.Where(z => z.EmployeeId.ToString() == getnextemp.ReportToEmpCode).ToList();
                            //Console.WriteLine("getReportto " + getreemp[0].EmployeeId.ToString() + " Isative = " + getreemp[0].IsActive.ToString());
                            if (getreemp.Count() == 0)
                            {
                                WriteLogFile("getnextemp.ReportToEmpCode1 "+getnextemp.ReportToEmpCode.ToString());
                                continue;
                            }
                            WriteLogFile("getreemp" + getreemp.Count());
                            var getisative = getreemp.Select(z => z.IsActive).ToList();
                            if (getisative == null)
                            {
                                WriteLogFile("getisative " + getisative);
                                continue;
                            }
                            WriteLogFile("getnextemp" + getnextemp.ReportToEmpCode[0].ToString());
                            WriteLogFile("getisative " + getisative.ToString());
                            if (getisative.Any(x => x == false))
                            {
                                additionalEmployees.AddRange(getreemp);
                                if (getreemp.Any(x => x.ReportToEmpCode == null || x.ReportToEmpCode == "0"))
                                {
                                    WriteLogFile("getnextemp.ReportToEmpCode2 " + getnextemp.ReportToEmpCode.ToString());
                                    continue;
                                }
                            }
                            else if (getisative.Any(x => x == true))
                            {
                                List<TRNMemo> getmemochange = getMemoAll.FindAll(z => z.MemoId == getdata.MemoId).ToList();
                                WriteLogFile("getmemochange " + getmemochange.ToString());
                                foreach (TRNMemo newMemo in getmemochange)
                                {
                                    var getviewempnew = getviewemp.FindAll(z => z.EmployeeId == getreemp[0].EmployeeId).ToList();
                                    newMemo.PersonWaitingId = getreemp[0].EmployeeId;
                                    newMemo.PersonWaiting = getreemp[0].NameTh;
                                    db.SubmitChanges();
                                    WriteLogFile("SubmitChanges NewMEmoPersonWaitingId and NewMemoPersonWaiting" + newMemo.PersonWaitingId + " " + newMemo.PersonWaiting.ToString());
                                    List<TRNLineApprove> getLineApprove = db.TRNLineApproves.Where(x => x.MemoId == getdata.MemoId && x.EmployeeId == getdata.PersonWaitingId).ToList();
                                    foreach (TRNLineApprove newLineApprove in getLineApprove)
                                    {
                                        newLineApprove.EmployeeId = getviewempnew[0].EmployeeId;
                                        newLineApprove.EmployeeCode = getviewempnew[0].EmployeeCode;
                                        newLineApprove.NameTh = getviewempnew[0].NameTh;
                                        newLineApprove.NameEn = getviewempnew[0].NameEn;
                                        newLineApprove.PositionTH = getviewempnew[0].PositionNameTh;
                                        newLineApprove.PositionEN = getviewempnew[0].PositionNameEn;

                                        db.SubmitChanges();
                                        WriteLogFile("SubmitChanges lineapprove" + newLineApprove.EmployeeId + newLineApprove.NameTh.ToString());
                                    }
                                    TRNActionHistory newActionHistory = new TRNActionHistory();

                                    newActionHistory.MemoId = newMemo.MemoId;
                                    WriteLogFile("MemoId " + newActionHistory.MemoId +" "+ i);
                                    newActionHistory.ActorId = 772;
                                    newActionHistory.ActorName = "System";
                                    newActionHistory.ActionDate = DateTime.Now;
                                    newActionHistory.ActionProcess = "Delegate";
                                    newActionHistory.Comment = getdata.PersonWaiting + " delegate to " + getviewempnew[0].NameTh;
                                    WriteLogFile(getdata.PersonWaiting + " delegate to " + getviewempnew[0].NameTh);
                                    
                                    Console.WriteLine(getdata.PersonWaiting  + " delegate to " + getviewempnew[0].NameTh);
                                    newActionHistory.ActionStatus = "Delegate";
                                    newActionHistory.SignatureId = 1;
                                    newActionHistory.Platform = "web";
                                    newActionHistory.IPAddress = "127.0.0.1";
                                    newActionHistory.ActorNameTh = "System";
                                    newActionHistory.ActorNameEn = "System";
                                    newActionHistory.ActorPositionId = 9587;
                                    newActionHistory.ActorPositionNameTh = "System";
                                    newActionHistory.ActorPositionNameEn = "System";
                                    newActionHistory.ActorDepartmentId = 11035;
                                    newActionHistory.ActorDepartmentNameEn = "System";
                                    newActionHistory.ActorDepartmentNameTh = "System";


                                    db.TRNActionHistories.InsertOnSubmit(newActionHistory);
                                    db.SubmitChanges();
                                    WriteLogFile("SubmitChanges" + getdata.PersonWaitingId + " delegate to " + getviewempnew[0].EmployeeId);
                                    Console.WriteLine("SubmitChanges");

                                }

                            }                           
                        }
                        if (additionalEmployees.Count > 0)
                        {
                            filteredData.Clear();
                            filteredData.AddRange(additionalEmployees);
                        }
                        else
                        {
                            hasNewEmployees = false;
                        }
                        if (j > 50)
                        {
                            break;
                        }
                        
                    }
                    Console.WriteLine("Finish " + getdata.MemoId);
                    WriteLogFile("Finish Memoid" + getdata.MemoId);
                    WriteLogFile("-----------------------------------------------------------------------------------------");
                }
                WriteLogFile("End Job");
            }
            catch(Exception ex)
            {
                WriteLogFile(ex.ToString());
            }
        }

    }
}

