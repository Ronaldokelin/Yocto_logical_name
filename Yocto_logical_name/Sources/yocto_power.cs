/*********************************************************************
 *
 *  $Id: yocto_power.cs 56107 2023-08-16 09:15:27Z seb $
 *
 *  Implements yFindPower(), the high-level API for Power functions
 *
 *  - - - - - - - - - License information: - - - - - - - - -
 *
 *  Copyright (C) 2011 and beyond by Yoctopuce Sarl, Switzerland.
 *
 *  Yoctopuce Sarl (hereafter Licensor) grants to you a perpetual
 *  non-exclusive license to use, modify, copy and integrate this
 *  file into your software for the sole purpose of interfacing
 *  with Yoctopuce products.
 *
 *  You may reproduce and distribute copies of this file in
 *  source or object form, as long as the sole purpose of this
 *  code is to interface with Yoctopuce products. You must retain
 *  this notice in the distributed source file.
 *
 *  You should refer to Yoctopuce General Terms and Conditions
 *  for additional information regarding your rights and
 *  obligations.
 *
 *  THE SOFTWARE AND DOCUMENTATION ARE PROVIDED 'AS IS' WITHOUT
 *  WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING
 *  WITHOUT LIMITATION, ANY WARRANTY OF MERCHANTABILITY, FITNESS
 *  FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO
 *  EVENT SHALL LICENSOR BE LIABLE FOR ANY INCIDENTAL, SPECIAL,
 *  INDIRECT OR CONSEQUENTIAL DAMAGES, LOST PROFITS OR LOST DATA,
 *  COST OF PROCUREMENT OF SUBSTITUTE GOODS, TECHNOLOGY OR
 *  SERVICES, ANY CLAIMS BY THIRD PARTIES (INCLUDING BUT NOT
 *  LIMITED TO ANY DEFENSE THEREOF), ANY CLAIMS FOR INDEMNITY OR
 *  CONTRIBUTION, OR OTHER SIMILAR COSTS, WHETHER ASSERTED ON THE
 *  BASIS OF CONTRACT, TORT (INCLUDING NEGLIGENCE), BREACH OF
 *  WARRANTY, OR OTHERWISE.
 *
 *********************************************************************/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using YDEV_DESCR = System.Int32;
using YFUN_DESCR = System.Int32;

#pragma warning disable 1591
//--- (YPower return codes)
//--- (end of YPower return codes)
//--- (YPower dlldef)
//--- (end of YPower dlldef)
//--- (YPower yapiwrapper)
//--- (end of YPower yapiwrapper)
//--- (YPower class start)
/**
 * <summary>
 *   The <c>YPower</c> class allows you to read and configure Yoctopuce electrical power sensors.
 * <para>
 *   It inherits from <c>YSensor</c> class the core functions to read measurements,
 *   to register callback functions, and to access the autonomous datalogger.
 *   This class adds the ability to access the energy counter and the power factor.
 * </para>
 * <para>
 * </para>
 * </summary>
 */
public class YPower : YSensor
{
//--- (end of YPower class start)
    //--- (YPower definitions)
    public new delegate void ValueCallback(YPower func, string value);
    public new delegate void TimedReportCallback(YPower func, YMeasure measure);

    public const double POWERFACTOR_INVALID = YAPI.INVALID_DOUBLE;
    public const double COSPHI_INVALID = YAPI.INVALID_DOUBLE;
    public const double METER_INVALID = YAPI.INVALID_DOUBLE;
    public const double DELIVEREDENERGYMETER_INVALID = YAPI.INVALID_DOUBLE;
    public const double RECEIVEDENERGYMETER_INVALID = YAPI.INVALID_DOUBLE;
    public const int METERTIMER_INVALID = YAPI.INVALID_UINT;
    protected double _powerFactor = POWERFACTOR_INVALID;
    protected double _cosPhi = COSPHI_INVALID;
    protected double _meter = METER_INVALID;
    protected double _deliveredEnergyMeter = DELIVEREDENERGYMETER_INVALID;
    protected double _receivedEnergyMeter = RECEIVEDENERGYMETER_INVALID;
    protected int _meterTimer = METERTIMER_INVALID;
    protected ValueCallback _valueCallbackPower = null;
    protected TimedReportCallback _timedReportCallbackPower = null;
    //--- (end of YPower definitions)

    public YPower(string func)
        : base(func)
    {
        _className = "Power";
        //--- (YPower attributes initialization)
        //--- (end of YPower attributes initialization)
    }

    //--- (YPower implementation)

    protected override void _parseAttr(YAPI.YJSONObject json_val)
    {
        if (json_val.has("powerFactor"))
        {
            _powerFactor = Math.Round(json_val.getDouble("powerFactor") / 65.536) / 1000.0;
        }
        if (json_val.has("cosPhi"))
        {
            _cosPhi = Math.Round(json_val.getDouble("cosPhi") / 65.536) / 1000.0;
        }
        if (json_val.has("meter"))
        {
            _meter = Math.Round(json_val.getDouble("meter") / 65.536) / 1000.0;
        }
        if (json_val.has("deliveredEnergyMeter"))
        {
            _deliveredEnergyMeter = Math.Round(json_val.getDouble("deliveredEnergyMeter") / 65.536) / 1000.0;
        }
        if (json_val.has("receivedEnergyMeter"))
        {
            _receivedEnergyMeter = Math.Round(json_val.getDouble("receivedEnergyMeter") / 65.536) / 1000.0;
        }
        if (json_val.has("meterTimer"))
        {
            _meterTimer = json_val.getInt("meterTimer");
        }
        base._parseAttr(json_val);
    }


    /**
     * <summary>
     *   Returns the power factor (PF), i.e.
     * <para>
     *   ratio between the active power consumed (in W)
     *   and the apparent power provided (VA).
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the power factor (PF), i.e
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPower.POWERFACTOR_INVALID</c>.
     * </para>
     */
    public double get_powerFactor()
    {
        double res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return POWERFACTOR_INVALID;
                }
            }
            res = this._powerFactor;
            if (res == POWERFACTOR_INVALID) {
                res = this._cosPhi;
            }
            res = Math.Round(res * 1000) / 1000;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the Displacement Power factor (DPF), i.e.
     * <para>
     *   cosine of the phase shift between
     *   the voltage and current fundamentals.
     *   On the Yocto-Watt (V1), the value returned by this method correponds to the
     *   power factor as this device is cannot estimate the true DPF.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the Displacement Power factor (DPF), i.e
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPower.COSPHI_INVALID</c>.
     * </para>
     */
    public double get_cosPhi()
    {
        double res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return COSPHI_INVALID;
                }
            }
            res = this._cosPhi;
        }
        return res;
    }

    public int set_meter(double newval)
    {
        string rest_val;
        lock (_thisLock) {
            rest_val = Math.Round(newval * 65536.0).ToString();
            return _setAttr("meter", rest_val);
        }
    }


    /**
     * <summary>
     *   Returns the energy counter, maintained by the wattmeter by integrating the
     *   power consumption over time.
     * <para>
     *   This is the sum of forward and backwad energy transfers,
     *   if you are insterested in only one direction, use  get_receivedEnergyMeter() or
     *   get_deliveredEnergyMeter(). Note that this counter is reset at each start of the device.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the energy counter, maintained by the wattmeter by integrating the
     *   power consumption over time
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPower.METER_INVALID</c>.
     * </para>
     */
    public double get_meter()
    {
        double res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return METER_INVALID;
                }
            }
            res = this._meter;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the energy counter, maintained by the wattmeter by integrating the power consumption over time,
     *   but only when positive.
     * <para>
     *   Note that this counter is reset at each start of the device.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the energy counter, maintained by the wattmeter by
     *   integrating the power consumption over time,
     *   but only when positive
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPower.DELIVEREDENERGYMETER_INVALID</c>.
     * </para>
     */
    public double get_deliveredEnergyMeter()
    {
        double res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return DELIVEREDENERGYMETER_INVALID;
                }
            }
            res = this._deliveredEnergyMeter;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the energy counter, maintained by the wattmeter by integrating the power consumption over time,
     *   but only when negative.
     * <para>
     *   Note that this counter is reset at each start of the device.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   a floating point number corresponding to the energy counter, maintained by the wattmeter by
     *   integrating the power consumption over time,
     *   but only when negative
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPower.RECEIVEDENERGYMETER_INVALID</c>.
     * </para>
     */
    public double get_receivedEnergyMeter()
    {
        double res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return RECEIVEDENERGYMETER_INVALID;
                }
            }
            res = this._receivedEnergyMeter;
        }
        return res;
    }


    /**
     * <summary>
     *   Returns the elapsed time since last energy counter reset, in seconds.
     * <para>
     * </para>
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the elapsed time since last energy counter reset, in seconds
     * </returns>
     * <para>
     *   On failure, throws an exception or returns <c>YPower.METERTIMER_INVALID</c>.
     * </para>
     */
    public int get_meterTimer()
    {
        int res;
        lock (_thisLock) {
            if (this._cacheExpiration <= YAPI.GetTickCount()) {
                if (this.load(YAPI._yapiContext.GetCacheValidity()) != YAPI.SUCCESS) {
                    return METERTIMER_INVALID;
                }
            }
            res = this._meterTimer;
        }
        return res;
    }


    /**
     * <summary>
     *   Retrieves a electrical power sensor for a given identifier.
     * <para>
     *   The identifier can be specified using several formats:
     * </para>
     * <para>
     * </para>
     * <para>
     *   - FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleSerialNumber.FunctionLogicalName
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionIdentifier
     * </para>
     * <para>
     *   - ModuleLogicalName.FunctionLogicalName
     * </para>
     * <para>
     * </para>
     * <para>
     *   This function does not require that the electrical power sensor is online at the time
     *   it is invoked. The returned object is nevertheless valid.
     *   Use the method <c>YPower.isOnline()</c> to test if the electrical power sensor is
     *   indeed online at a given time. In case of ambiguity when looking for
     *   a electrical power sensor by logical name, no error is notified: the first instance
     *   found is returned. The search is performed first by hardware name,
     *   then by logical name.
     * </para>
     * <para>
     *   If a call to this object's is_online() method returns FALSE although
     *   you are certain that the matching device is plugged, make sure that you did
     *   call registerHub() at application initialization time.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="func">
     *   a string that uniquely characterizes the electrical power sensor, for instance
     *   <c>YWATTMK1.power</c>.
     * </param>
     * <returns>
     *   a <c>YPower</c> object allowing you to drive the electrical power sensor.
     * </returns>
     */
    public static YPower FindPower(string func)
    {
        YPower obj;
        lock (YAPI.globalLock) {
            obj = (YPower) YFunction._FindFromCache("Power", func);
            if (obj == null) {
                obj = new YPower(func);
                YFunction._AddToCache("Power", func, obj);
            }
        }
        return obj;
    }


    /**
     * <summary>
     *   Registers the callback function that is invoked on every change of advertised value.
     * <para>
     *   The callback is invoked only during the execution of <c>ySleep</c> or <c>yHandleEvents</c>.
     *   This provides control over the time when the callback is triggered. For good responsiveness, remember to call
     *   one of these two functions periodically. To unregister a callback, pass a null pointer as argument.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="callback">
     *   the callback function to call, or a null pointer. The callback function should take two
     *   arguments: the function object of which the value has changed, and the character string describing
     *   the new advertised value.
     * @noreturn
     * </param>
     */
    public int registerValueCallback(ValueCallback callback)
    {
        string val;
        if (callback != null) {
            YFunction._UpdateValueCallbackList(this, true);
        } else {
            YFunction._UpdateValueCallbackList(this, false);
        }
        this._valueCallbackPower = callback;
        // Immediately invoke value callback with current value
        if (callback != null && this.isOnline()) {
            val = this._advertisedValue;
            if (!(val == "")) {
                this._invokeValueCallback(val);
            }
        }
        return 0;
    }


    public override int _invokeValueCallback(string value)
    {
        if (this._valueCallbackPower != null) {
            this._valueCallbackPower(this, value);
        } else {
            base._invokeValueCallback(value);
        }
        return 0;
    }


    /**
     * <summary>
     *   Registers the callback function that is invoked on every periodic timed notification.
     * <para>
     *   The callback is invoked only during the execution of <c>ySleep</c> or <c>yHandleEvents</c>.
     *   This provides control over the time when the callback is triggered. For good responsiveness, remember to call
     *   one of these two functions periodically. To unregister a callback, pass a null pointer as argument.
     * </para>
     * <para>
     * </para>
     * </summary>
     * <param name="callback">
     *   the callback function to call, or a null pointer. The callback function should take two
     *   arguments: the function object of which the value has changed, and an <c>YMeasure</c> object describing
     *   the new advertised value.
     * @noreturn
     * </param>
     */
    public int registerTimedReportCallback(TimedReportCallback callback)
    {
        YSensor sensor;
        sensor = this;
        if (callback != null) {
            YFunction._UpdateTimedReportCallbackList(sensor, true);
        } else {
            YFunction._UpdateTimedReportCallbackList(sensor, false);
        }
        this._timedReportCallbackPower = callback;
        return 0;
    }


    public override int _invokeTimedReportCallback(YMeasure value)
    {
        if (this._timedReportCallbackPower != null) {
            this._timedReportCallbackPower(this, value);
        } else {
            base._invokeTimedReportCallback(value);
        }
        return 0;
    }


    /**
     * <summary>
     *   Resets the energy counters.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual int reset()
    {
        return this.set_meter(0);
    }

    /**
     * <summary>
     *   Continues the enumeration of electrical power sensors started using <c>yFirstPower()</c>.
     * <para>
     *   Caution: You can't make any assumption about the returned electrical power sensors order.
     *   If you want to find a specific a electrical power sensor, use <c>Power.findPower()</c>
     *   and a hardwareID or a logical name.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPower</c> object, corresponding to
     *   a electrical power sensor currently online, or a <c>null</c> pointer
     *   if there are no more electrical power sensors to enumerate.
     * </returns>
     */
    public YPower nextPower()
    {
        string hwid = "";
        if (YAPI.YISERR(_nextFunction(ref hwid)))
            return null;
        if (hwid == "")
            return null;
        return FindPower(hwid);
    }

    //--- (end of YPower implementation)

    //--- (YPower functions)

    /**
     * <summary>
     *   Starts the enumeration of electrical power sensors currently accessible.
     * <para>
     *   Use the method <c>YPower.nextPower()</c> to iterate on
     *   next electrical power sensors.
     * </para>
     * </summary>
     * <returns>
     *   a pointer to a <c>YPower</c> object, corresponding to
     *   the first electrical power sensor currently online, or a <c>null</c> pointer
     *   if there are none.
     * </returns>
     */
    public static YPower FirstPower()
    {
        YFUN_DESCR[] v_fundescr = new YFUN_DESCR[1];
        YDEV_DESCR dev = default(YDEV_DESCR);
        int neededsize = 0;
        int err = 0;
        string serial = null;
        string funcId = null;
        string funcName = null;
        string funcVal = null;
        string errmsg = "";
        int size = Marshal.SizeOf(v_fundescr[0]);
        IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(v_fundescr[0]));
        err = YAPI.apiGetFunctionsByClass("Power", 0, p, size, ref neededsize, ref errmsg);
        Marshal.Copy(p, v_fundescr, 0, 1);
        Marshal.FreeHGlobal(p);
        if ((YAPI.YISERR(err) | (neededsize == 0)))
            return null;
        serial = "";
        funcId = "";
        funcName = "";
        funcVal = "";
        errmsg = "";
        if ((YAPI.YISERR(YAPI.yapiGetFunctionInfo(v_fundescr[0], ref dev, ref serial, ref funcId, ref funcName, ref funcVal, ref errmsg))))
            return null;
        return FindPower(serial + "." + funcId);
    }

    //--- (end of YPower functions)
}
#pragma warning restore 1591

