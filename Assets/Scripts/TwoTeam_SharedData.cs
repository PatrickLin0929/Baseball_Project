using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoTeam_SharedData : MonoBehaviour
{
    // Start is called before the first frame update
    // Player selection list
    public static List<int> teamAPlayerList = new List<int>();
    public static List<int> teamBPlayerList = new List<int>();

    // Player ordering list
    public static List<int> teamAPlayerOrderedList = new List<int>();
    public static List<int> teamBPlayerOrderedList = new List<int>();

    // List of player names to find index in the matrix database
    public static List<string> playerList = new List<string> {"1 曾子祐", "4 吉力吉撈．鞏冠", "5 梁家榮",
                                                              "9 王柏融", "24 陳傑憲", "29 陳俊秀", 
                                                              "29 申皓瑋", "31 林智勝", "32 蘇智傑", 
                                                              "35 王正棠", "39 林立", "46 劉基鴻", 
                                                              "74 許基宏", "77 林安可", "85 朱育賢", 
                                                              "92 岳政華", "94 魔鷹", "99 張育成"};

    // Format = Height, Weight, Batting Hand, Age, Experience, AVG, HR, RBI, OBP, SLG
    /*  身高
        體重
        投打習慣
        年齡
        經驗值
        全壘打數 (HR)
        打點 (RBI)
        上壘率 (OBP)
        長打率 (SLG)
        打擊率 (AVG)*/
    public static List<List<string>> playerList_Info = new List<List<string>> { 
        new List<string> { "178公分", "74公斤", "右投右打", "2003/09/16 (21歲)", "2024/04/03 (<1年)", "3", "20", "0.364", "0.394", "0.310"},
        new List<string> { "180公分", "104公斤", "右投右打", "1994/03/13 (30歲)", "2021/08/25 (3年)", "14", "47", "0.319", "0.465", "0.260"},
        new List<string> { "180公分", "90公斤", "右投左打", "1995/03/25 (29歲)", "2013/10/15 (11年)", "3", "28", "0.332", "0.361", "0.275"},

        new List<string> { "182公分", "91公斤", "右投左打", "1993/09/09 (31歲)", "2015/09/02 (9年)", "6", "35", "0.393", "0.398", "0.275"},
        new List<string> { "173公分", "73公斤", "右投左打", "1994/01/07 (30歲)", "2016/07/29 (8年)", "1", "30", "0.449", "0.449", "0.360"},
        new List<string> { "183公分", "106公斤", "右投右打", "1988/11/01 (36歲)", "2014/07/30 (10年)", "6", "33", "0.418", "0.473", "0.306"},

        new List<string> { "183公分", "75公斤", "右投右打", "1997/09/12 (27歲)", "2017/05/20 (7年)", "6", "33", "0.300", "0.393", "0.249"},
        new List<string> { "183公分", "108公斤", "右投右打", "1982/01/01 (42歲)", "2004/06/03 (20年)", "1", "5", "0.291", "0.412", "0.235"},
        new List<string> { "180公分", "88公斤", "右投左打", "1994/07/28 (30歲)", "2016/07/29 (8年)", "1", "7", "0.333", "0.389", "0.296"},

        new List<string> { "177公分", "81公斤", "右投左打", "1995/09/17 (29歲)", "2018/08/22 (6年)", "6", "36", "0.385", "0.517", "0.352"},
        new List<string> { "182公分", "86公斤", "右投右打", "1996/01/01 (28歲)", "2017/08/30 (7年)", "1", "13", "0.409", "0.403", "0.347"},
        new List<string> { "180公分", "88公斤", "右投右打", "2000/11/03 (24歲)", "2021/03/14 (3年)", "4", "34", "0.311", "0.351", "0.243"},

        new List<string> { "189公分", "108公斤", "右投左打", "1992/07/22 (32歲)", "2014/09/03 (10年)", "5", "31", "0.343", "0.416", "0.283"},
        new List<string> { "184公分", "90公斤", "左投左打", "1997/05/19 (27歲)", "2019/08/10 (5年)", "13", "64", "0.339", "0.477", "0.257"},
        new List<string> { "188公分", "100公斤", "左投左打", "1991/11/26 (33歲)", "2016/03/19 (8年)", "6", "26", "0.338", "0.419", "0.293"},

        new List<string> { "176公分", "78公斤", "左投左打", "2001/01/29 (23歲)", "2020/09/26 (4年)", "2", "22", "0.327", "0.362", "0.273"},
        new List<string> { "201公分", "117公斤", "右投左打", "1991/08/09 (33歲)", "2024/04/03 (<1年)", "16", "50", "0.398", "0.581", "0.326"},
        new List<string> { "185公分", "95公斤", "右投右打", "1995/08/18 (29歲)", "2024/07/12 (<1年)", "0", "0", "0.250", "0.500", "0.250"},
    };
    public static List<List<double>> playerMatrix_Fastball = new List<List<double>> { 
        new List<double> { 0.18, 0.17, 0.14, 0.01, 0.04, 0.46 },
        new List<double> { 0.18, 0.17, 0.14, 0.01, 0.04, 0.46 },
        new List<double> { 0.18, 0.17, 0.14, 0.01, 0.04, 0.46 },

        new List<double> { 0.18, 0.17, 0.14, 0.01, 0.04, 0.46 },
        new List<double> { 0.18, 0.17, 0.14, 0.01, 0.04, 0.46 },
        new List<double> { 0.18, 0.17, 0.14, 0.01, 0.04, 0.46 },

        new List<double> { 0.18, 0.17, 0.14, 0.01, 0.04, 0.46 },
        new List<double> { 0.18, 0.17, 0.14, 0.01, 0.04, 0.46 },
        new List<double> { 0.18, 0.17, 0.14, 0.01, 0.04, 0.46 },

        new List<double> { 0.15, 0.165, 0.14, 0.015, 0.05, 0.48 },
        new List<double> { 0.15, 0.165, 0.14, 0.015, 0.05, 0.48 },
        new List<double> { 0.15, 0.165, 0.14, 0.015, 0.05, 0.48 },

        new List<double> { 0.15, 0.165, 0.14, 0.015, 0.05, 0.48 },
        new List<double> { 0.15, 0.165, 0.14, 0.015, 0.05, 0.48 },
        new List<double> { 0.15, 0.165, 0.14, 0.015, 0.05, 0.48 },

        new List<double> { 0.15, 0.165, 0.14, 0.015, 0.05, 0.48 },
        new List<double> { 0.15, 0.165, 0.14, 0.015, 0.05, 0.48 },
        new List<double> { 0.15, 0.165, 0.14, 0.015, 0.05, 0.48 }
    };
    public static List<List<double>> playerMatrix_Curveball = new List<List<double>> {
        new List<double> { 0.25, 0.145, 0.15, 0.005, 0.05, 0.4 },
        new List<double> { 0.25, 0.145, 0.15, 0.005, 0.05, 0.4 },
        new List<double> { 0.25, 0.145, 0.15, 0.005, 0.05, 0.4 },

        new List<double> { 0.25, 0.145, 0.15, 0.005, 0.05, 0.4 },
        new List<double> { 0.25, 0.145, 0.15, 0.005, 0.05, 0.4 },
        new List<double> { 0.25, 0.145, 0.15, 0.005, 0.05, 0.4 },

        new List<double> { 0.25, 0.145, 0.15, 0.005, 0.05, 0.4 },
        new List<double> { 0.25, 0.145, 0.15, 0.005, 0.05, 0.4 },
        new List<double> { 0.25, 0.145, 0.15, 0.005, 0.05, 0.4 },

        new List<double> { 0.2, 0.1, 0.11, 0.02, 0.05, 0.52 },
        new List<double> { 0.2, 0.1, 0.11, 0.02, 0.05, 0.52 },
        new List<double> { 0.2, 0.1, 0.11, 0.02, 0.05, 0.52 },

        new List<double> { 0.2, 0.1, 0.11, 0.02, 0.05, 0.52 },
        new List<double> { 0.2, 0.1, 0.11, 0.02, 0.05, 0.52 },
        new List<double> { 0.2, 0.1, 0.11, 0.02, 0.05, 0.52 },

        new List<double> { 0.2, 0.1, 0.11, 0.02, 0.05, 0.52 },
        new List<double> { 0.2, 0.1, 0.11, 0.02, 0.05, 0.52 },
        new List<double> { 0.2, 0.1, 0.11, 0.02, 0.05, 0.52 }
    };
    public static List<List<double>> playerMatrix_Slider = new List<List<double>> {
        new List<double> { 0.22, 0.156, 0.07, 0.004, 0.01, 0.54 },
        new List<double> { 0.22, 0.156, 0.07, 0.004, 0.01, 0.54 },
        new List<double> { 0.22, 0.156, 0.07, 0.004, 0.01, 0.54 },

        new List<double> { 0.22, 0.156, 0.07, 0.004, 0.01, 0.54 },
        new List<double> { 0.22, 0.156, 0.07, 0.004, 0.01, 0.54 },
        new List<double> { 0.22, 0.156, 0.07, 0.004, 0.01, 0.54 },

        new List<double> { 0.22, 0.156, 0.07, 0.004, 0.01, 0.54 },
        new List<double> { 0.22, 0.156, 0.07, 0.004, 0.01, 0.54 },
        new List<double> { 0.22, 0.156, 0.07, 0.004, 0.01, 0.54 },

        new List<double> { 0.25, 0.14, 0.05, 0.01, 0.03, 0.52 },
        new List<double> { 0.25, 0.14, 0.05, 0.01, 0.03, 0.52 },
        new List<double> { 0.25, 0.14, 0.05, 0.01, 0.03, 0.52 },

        new List<double> { 0.25, 0.14, 0.05, 0.01, 0.03, 0.52 },
        new List<double> { 0.25, 0.14, 0.05, 0.01, 0.03, 0.52 },
        new List<double> { 0.25, 0.14, 0.05, 0.01, 0.03, 0.52 },

        new List<double> { 0.25, 0.14, 0.05, 0.01, 0.03, 0.52 },
        new List<double> { 0.25, 0.14, 0.05, 0.01, 0.03, 0.52 },
        new List<double> { 0.25, 0.14, 0.05, 0.01, 0.03, 0.52 }
    };

    

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
