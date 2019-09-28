-- Lua 5.3 standard library

-- For more information, see <<Lua 5.3 Reference Manual>>.
-- https://www.lua.org/manual/5.3/manual.html

-- function assert(v, opt message)
function assert(v, message)
end

-- function collectgarbage(opt option, opt arg)
function collectgarbage(option, arg)
end

-- function dofile(opt filename)
function dofile(filename)
end

-- function error(message, opt level)
function error(message, level)
end

function getmetatable(object)
end

function ipairs(t)
end

-- function load(chunk, opt chunkname, opt mode, opt env)
function load(chunk, chunkname, mode, env)
end

-- function loadfile(opt filename, opt mode, opt env)
function loadfile(filename, mode, env)
end

-- function next(table, opt index)
function next(table, index)
end

function pairs(t)
end

function pcall(f, ...)
end

function print(...)
end

function rawequal(v1, v2)
end

function rawget(table, index)
end

function rawlen(v)
end

function rawset(table, index, value)
end

function require(modname)
end

function select(index, ...)
end

function setmetatable(table, metatable)
end

-- function tonumber(e, opt base)
function tonumber(e, base)
end

function tostring(v)
end

function type(v)
end

function xpcall(f, msgh, ...)
end

-- coroutine

coroutine = {}

function coroutine.create(f)
end

function coroutine.isyieldable()
end

function coroutine.resume(co, ...)
end

function coroutine.running()
end

function coroutine.status(co)
end

function coroutine.wrap(f)
end

function coroutine.yield(...)
end

package = {}

function package.loadlib(libname, funcname)
end

-- function package.searchpath(name, path, opt sep, opt rep)
function package.searchpath(name, path, sep, rep)
end

string = {}

-- function string.byte(s, opt i, opt j)
function string.byte(s, i, j)
end

function string.char(...)
end

-- function string.dump(func, opt strip)
function string.dump(func, strip)
end

-- function string.find(s, pattern, opt init, opt plain)
function string.find(s, pattern, init, plain)
end

function string.format(formatstring, ...)
end

function string.gmatch(s, pattern)
end

-- function string.gsub(s, pattern, repl, opt n)
function string.gsub(s, pattern, repl, n)
end

function string.len(s)
end

function string.lower(s)
end

-- function string.match(s, pattern, opt init)
function string.match(s, pattern, init)
end

function string.pack(fmt, v1, v2, ...)
end

function string.packsize(fmt)
end

-- function string.rep(s, n, opt sep)
function string.rep(s, n, sep)
end

function string.reverse(s)
end

-- function string.sub(s, i, opt j)
function string.sub(s, i, j)
end

-- function string.unpack(fmt, s, opt pos)
function string.unpack(fmt, s, pos)
end

function string.upper(s)
end

utf8 = {}

function utf8.char(...)
end

-- function utf8.codepoint(s, opt i, opt j)
function utf8.codepoint(s, i, j)
end

function utf8.codes(s)
end

-- function utf8.len(s, opt i, opt j)
function utf8.len(s, i, j)
end

-- function utf8.offset(s, n, opt i)
function utf8.offset(s, n, i)
end

table = {}

-- function table.concat(list, opt sep, opt i, opt j)
function table.concat(list, sep, i, j)
end

-- function table.insert(list, opt pos, value)
function table.insert(list, pos, value)
end

-- function table.move(a1, f, e, t, opt a2)
function table.move(a1, f, e, t, a2)
end

function table.pack(...)
end

-- function table.remove(list, opt pos)
function table.remove(list, pos)
end

-- function table.sort(list, opt comp)
function table.sort(list, comp)
end

-- function table.unpack(list, opt i, opt j)
function table.unpack(list, i, j)
end

math = {}

function math.abs(x)
end

function math.acos(x)
end

function math.asin(x)
end

-- function math.atan(y, opt x)
function math.atan(y, x)
end

function math.ceil(x)
end

function math.cos(x)
end

function math.deg(x)
end

function math.exp(x)
end

function math.floor(x)
end

function math.fmod(x, y)
end

-- function math.log(x, opt base)
function math.log(x, base)
end

function math.max(x, ...)
end

function math.min(x, ...)
end

function math.modf(x)
end

function math.rad(x)
end

-- function math.random(opt m, opt n)
function math.random(m, n)
end

function math.randomseed(x)
end

function math.sin(x)
end

function math.sqrt(x)
end

function math.tan(x)
end

function math.tointeger(x)
end

function math.type(x)
end

function math.ult(m, n)
end

math.huge = 0

math.maxinteger = 0

math.mininteger = 0

math.pi = 0

io = {}

-- function io.close(opt file)
function io.close(file)
end

function io.flush()
end

-- function io.input(opt file)
function io.input(file)
end

-- function io.lines(opt filename, ...)
function io.lines(filename, ...)
end

-- function io.open(filename, opt mode) : File
function io.open(filename, mode)
end

-- function io.output(opt file)
function io.output(file)
end

-- function io.popen(prog, opt mode)
function io.popen(prog, mode)
end

function io.read(...)
end

function io.tmpfile()
end

function io.type(obj)
end

function io.write(...)
end

local File = {}

function File:close()
end

function File:flush()
end

function File:lines(...)
end

function File:read(...)
end

-- function File:seek(opt whence, opt offset)
function File:seek(whence, offset)
end

-- function File:setvbuf(mode, opt size)
function File:setvbuf(mode, size)
end

function File:write(...)
end

os = {}

function os.clock()
end

-- function os.date(opt format, opt time)
function os.date(format, time)
end

function os.difftime(t2, t1)
end

-- function os.execute(opt command)
function os.execute(command)
end

-- function os.exit(opt code, opt close)
function os.exit(code, close)
end

function os.getenv(varname)
end

function os.remove(filename)
end

function os.rename(oldname, newname)
end

-- function os.setlocale(locale, opt category)
function os.setlocale(locale, category)
end

-- function os.time(opt table)
function os.time(table)
end

function os.tmpname()
end

debug = {}

function debug.debug()
end

-- function debug.gethook(opt thread)
function debug.gethook(thread)
end

-- function debug.getinfo(opt thread, func, opt what)
function debug.getinfo(thread, func, what)
end

-- function debug.getlocal(opt thread, func, local_name)
function debug.getlocal(thread, func, local_name)
end

function debug.getmetatable(value)
end

function debug.getregistry()
end

function debug.getupvalue(f, up)
end

function debug.getuservalue(u)
end

-- function debug.sethook(opt thread, hook, mask, opt count)
function debug.sethook(thread, hook, mask, count)
end

-- function debug.setlocal(opt thread, level, local_name, value)
function debug.setlocal(thread, level, local_name, value)
end

function debug.setmetatable(value, table)
end

function debug.setupvalue(f, up, value)
end

function debug.setuservalue(udata, value)
end

-- function debug.traceback(opt thread, opt message, opt level)
function debug.traceback(thread, message, level)
end

function debug.upvalueid(f, n)
end

function debug.upvaluejoin(f1, n1, f2, n2)
end