---@module LuaDebuggee
local LuaDebuggee = {}

---@field public DEFAULT_DEBUG_PORT int @调试器默认端口号
LuaDebuggee.DEFAULT_DEBUG_PORT = 9826

-- 开始调试(连接调试器)
---@param ip : string   @调试器ip地址,本机调试可填写'127.0.0.1'
---@param port : int    @调试器端口地址,默认为9826,多开时需填写调试器标题栏中显示的端口号
function LuaDebuggee.StartDebug(ip, port)
end

-- 开始性能测试(连接调试器)
---@param ip : string   @调试器ip地址,本机调试可填写'127.0.0.1'
---@param port : int    @调试器端口地址,默认为9826,多开时需填写调试器标题栏中显示的端口号
function LuaDebuggee.StartProfile(ip, port)
end

-- 停止调试或性能测试
function LuaDebuggee.Stop()
end

-- 向调试器打印日志
---@param color : int       @日志颜色,如0xFFFFFFFF表示白色
---@param string : string   @日志内容
function LuaDebuggee.Print(color, string)
end

-- 返回以秒为单位的高精度时间
---@return number
function LuaDebuggee.Time()
end

-- 线程休眠timeMsToSleep毫秒
---@param timeMsToSleep : int
function LuaDebuggee.Sleep(timeMsToSleep)
end

---@field public DebugOnPanic function @错误处理函数，可以在调试器中触发Lua异常
LuaDebuggee.DebugOnPanic = function(errorMessage)
end

return LuaDebuggee