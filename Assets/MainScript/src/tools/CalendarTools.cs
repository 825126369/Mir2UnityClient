using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarTools
{
    public static int GetWeekDay(int nYear, int nMonth, int nDay)
	{
        DateTime dateTime = new DateTime(nYear, nMonth, nDay);
		return (int)dateTime.DayOfWeek;
	}

    public static int GetSumDayOfMonth(int nYear, int nMonth)
    {
        //DateTime mDateTime = new DateTime(nYear, nMonth + 1, 0);
        return DateTime.DaysInMonth(nYear, nMonth);
    }

    public static DateTime[] GetMonthBeginEndDay(int nYear, int nMonth)
    {
        int nLastMonth_Year = nYear;
        int nLastMonth_Month = nMonth - 1;
        if (nMonth - 1 <= 0)
        {
            nLastMonth_Year = nYear - 1;
            nLastMonth_Month = 12;
        }

        int nNextMonth_Year = nYear;
        int nNextMonth_Month = nMonth + 1;
        if (nMonth + 1 > 12)
        {
            nNextMonth_Year = nYear + 1;
            nNextMonth_Month = 1;
        }

        int nLastMonthSumDays = GetSumDayOfMonth(nLastMonth_Year, nLastMonth_Month);
        int nThisMonthSumDays = GetSumDayOfMonth(nYear, nMonth);

        int nDay1WeekOfDay = GetWeekDay(nYear, nMonth, 1);
        DateTime[] dayArray = new DateTime[42];

        int Offset = nDay1WeekOfDay;
        for (int i = 0; i < 42; i++)
        {
            DateTime mDateTime;
            if (i < Offset)
            {
                int nDay = nLastMonthSumDays - Offset + i + 1;
                mDateTime = new DateTime(nLastMonth_Year, nLastMonth_Month, nDay);
            }
            else
            {
                int nDay = i - Offset + 1;
                if (nDay <= nThisMonthSumDays)
                {
                    mDateTime = new DateTime(nYear, nMonth, nDay);
                }
                else
                {
                    nDay = nDay - nThisMonthSumDays;
                    mDateTime = new DateTime(nNextMonth_Year, nNextMonth_Month, nDay);
                }
            }
            dayArray[i] = mDateTime;
        }

        return dayArray;
    }
    
}

