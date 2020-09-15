-- poemfile = io.open("poem.txt", "rb")
-- chaiziFile = io.open("chaizi-jt.txt", "rb")

local poemTable = {}        -- 诗词原文件,去重
local chaiziTable = {}      -- 包含步数信息的拆字表
local reverseTable = {}     -- 包含步数信息的翻转表
local poemPlusTable = {}    -- 包含诗词源文件和成分偏旁部首的表
local file4Table = {}       -- 封闭性检测通过后的表

-- 加载poem.txt文件，并存储到poemTable中
function LoadPoemfile()
    for line in io.lines("poem.txt") do
        local i = 1
        while i < #line do
            local str = string.sub(line,i,i+1)  -- 中文长度为2
            i = i + 2
            if IsContainTarget(poemTable,str) == false then
                table.insert(poemTable ,str)
            end
        end
        -- print(line)
    end
    -- log_tree(poemTable)
end

function IsContainTarget(targetTable,str)
    for _,v in pairs(targetTable) do
        if v == str then
            return true
        end
    end

    -- log_tree(targetTable)
    return false
end

-- 生成reverseTable和chaiziTable
function LoadChaiziFile()
    for line in io.lines("chaizi-jt.txt") do    -- line 拆字表的每一行
        -- local subTable = string.split(line, ",")
        -- 生成结构：
        -- {
        --     七,      -- key_str
        --     一 乚,   
        --     丿 乚,
        -- }
        local subTable = Split(line, "	")      
        local key_str = subTable[1]             -- 母字
        local tempTable = {}
        for i = 2,#subTable do
            local wordTable = Split(subTable[i], " ")
            for i,v in pairs(wordTable) do
                if reverseTable[v] == nil then
                    reverseTable[v] = {}
                end
                local wrd = {}
                table.insert(wrd,#wordTable)
                table.insert(wrd,key_str)
                table.insert(reverseTable[v],wrd)
            end
            table.insert( tempTable, #wordTable, wordTable )
            -- print("wordTable",#wordTable)
        end
        chaiziTable[key_str] = tempTable
    end
    -- log_tree(chaiziTable)
    -- log_tree(reverseTable)
end

-- 生成中间文件2
function InitPoemPlusTable()
    for _,v in pairs(poemTable) do
        if chaiziTable[v] ~= nil then
            table.insert(poemPlusTable,v)
            for k,s in pairs(chaiziTable[v]) do
                for index,value in pairs(s) do
                    if IsContainTarget(poemPlusTable,value) == false then
                        table.insert(poemPlusTable, value)
                    end
                end
            end
        end
    end
    -- log_tree(poemPlusTable)
    Write2PoemPlusFile()
end

function Write2PoemPlusFile()
    PoemPlusFile = io.open("poemPlusFile.txt","w+")

    for _,v in pairs(poemPlusTable) do
        local str = v.."\n"
        PoemPlusFile:write(str)
    end

    PoemPlusFile:close()
end

function OutputFile3()
    File3 = io.open("file3.csv", "w+")

    for _,v in pairs(poemPlusTable) do
        if reverseTable[v] ~= nil then
            local str = v..","
            for i,value in pairs(reverseTable[v]) do                
                for k,s in pairs(value) do
                    if type(s) ~= "number" then
                        str = str..tostring(s)..","      -- TODO：此处使用concat提升性能
                    end
                end
            end
            str = str.."\n"
            File3:write(str)
        end
    end

    File3:close()
end

function OutputFile4()
    for lines in io.lines("file3.csv") do
        local subTable = Split(lines, ",")
        for i,v in pairs(subTable) do
            if file4Table[v] == nil then
                file4Table[v] = 1
            else
                file4Table[v] = file4Table[v] + 1
            end
        end
    end

    -- log_tree(file4Table)
    
    File4 = io.open("file4.txt", "w+")
    for i,v in pairs(file4Table) do
        if v > 1 then
            File4:write(i)
            File4:write("\n")
        else
            -- table.remove(file4Table,i)
            file4Table[i] = nil
        end
        -- File4:write("\n")
    end
    File4:close()
end

function OutputFile5()
    -- log_tree(file4Table)
    local finalTable = {}
    for lines in io.lines("file3.csv") do
        local subTable = Split(lines, ",")
        -- log_tree(subTable)
        finalTable[subTable[1]] = {}
        for i = 2, #subTable do
            -- print(subTable[i])
            -- if IsContainTarget(file4Table, subTable[i]) then
            if file4Table[subTable[i]] ~= nil then
                table.insert(finalTable[subTable[1]],subTable[i])
                -- print("contain target")
            end
        end
    end

    File5 = io.open("file5.txt", "w+")
    for i,v in pairs(finalTable) do
        local str = i
        for k,s in pairs(v) do
            str = str..","..s
        end
        str = str.."\n"

        File5:write(str)
    end

    File5:close()
    -- log_tree(finalTable)
end

function write2File()
    resultFile = io.open("result.csv","w+")
    
    for i,v in pairs(chaiziTable) do
        local str = nil
        str = i..","
        local tempbool = false
        for k,s in pairs(v) do
            -- if tempbool == false then
                str = str..k
            --     tempbool = true
            -- end
            for a,b in pairs(s) do
                str = str..b
            end
            str = str.."," 
        end
        str = str.."\n"
        resultFile:write(str)      
    end
    resultFile:close()
end

function write2PoemFile()
    PoemResultFile = io.open("poemResult.csv", "w+")

    for _,word in pairs(poemTable) do
        -- print("find"..word)
        for i,v in pairs(chaiziTable) do
            if i == word then
                local str = nil
                str = i..","
                local tempbool = false
                for k,s in pairs(v) do
                    -- if tempbool == false then
                        str = str..k
                    --     tempbool = true
                    -- end
                    for a,b in pairs(s) do
                        str = str..b
                    end
                    str = str.."," 
                end
                str = str.."\n"
                PoemResultFile:write(str)    
            end
        end
    end
    PoemResultFile:close()
end

function OutputReverseTable()
    reverseTableFile = io.open("reverseTable.csv", "w+")
    for i,v in pairs(reverseTable) do
        local str = i..","
        for k,s in pairs(v) do
            str = str..s[1]..s[2]..","
        end
        str = str.."\n"
        reverseTableFile:write(str)
    end
    reverseTableFile:close()
end

function OutputReverseTableCSV()
    PoemReverseTableFile = io.open("PoemReverseTable.csv", "w+")
    for _,word in pairs(poemTable) do
        for i,v in pairs(reverseTable) do
            if i == word then
                -- print("find"..word)
                local str = i..","
                for k,s in pairs(v) do
                    str = str..s[1]..s[2]..","      -- TODO：此处使用concat提升性能
                end
                str = str.."\n"
                -- print(str)
                PoemReverseTableFile:write(str)
            end
        end
    end
    PoemReverseTableFile:close()
end

function OutputReverseTablePlusCSV()
    PoemReverseTableFile = io.open("PoemReverseTablePlus.csv", "w+")
    for _,word in pairs(poemPlusTable) do
        for i,v in pairs(reverseTable) do
            if i == word then
                -- print("find"..word)
                local str = i..","
                for k,s in pairs(v) do
                    str = str..s[1]..s[2]..","      -- TODO：此处使用concat提升性能
                end
                str = str.."\n"
                -- print(str)
                PoemReverseTableFile:write(str)
            end
        end
    end
    PoemReverseTableFile:close()
end

local function var2string(var)
	local text = tostring(var);
	text = string.gsub(text, "\r", "\\r");
	text = string.gsub(text, "\n", "\\n");
	text = string.gsub(text, "%c", "\\^");
	return text;
end

local function tab2string(visited, path, base, tab)
    local pre = visited[tab];
    if pre then
        return pre;
    end

	visited[tab] = path;

	local size = 0;
    for k, v in pairs(tab) do
        size = size + 1;
    end

    if size == 0 then
        return "{ }";
    end

    local lines = {};
    local idx = 1;
	for k, v in pairs(tab) do
        local vtype = type(v);
        local header = base..(idx < size and "├─ " or "└─ ")..var2string(k);
        if vtype == "table" then
        	local vpath = visited[v];
        	if vpath then
        		lines[#lines + 1] = header..": "..vpath;
        	else
                local out = tab2string(visited, path.."/"..var2string(k), base..(idx < size and "│  " or "   "), v);
                if type(out) == "string" then
                    lines[#lines + 1] = header..": "..out;
                else
                    lines[#lines + 1] = header;
                    for _, one in ipairs(out) do
                        table.insert(lines, one);
                    end
                end
            end
        else
            local txt = var2string(v);
            if vtype == "string" then
                txt = '\"'..txt..'\"';
            end
            lines[#lines + 1] = header..": "..txt;
        end
        idx = idx + 1;
	end

    return lines;
end

-- 树状输出表格函数
function log_tree(desc, var)
    if var == nil then
        var = desc;
    end
    if type(var) ~= "table" then
        print(var2string(desc)..": "..var2string(var));
        return;
    end

    local visited = {};
    local out = tab2string(visited, "", "", var);

    if type(out) == "string" then
        print(var2string(desc)..": "..out);
        return;
    end

    print(var2string(desc));
    for i, line in ipairs(out) do
        print(line);
    end
end

-- 字符串分割函数
function Split(szFullString, szSeparator)
    local nFindStartIndex = 1
    local nSplitIndex = 1
    local nSplitArray = {}
    while true do
       local nFindLastIndex = string.find(szFullString, szSeparator, nFindStartIndex)
       if not nFindLastIndex then
        nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, string.len(szFullString))
        break
       end
       nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, nFindLastIndex - 1)
       nFindStartIndex = nFindLastIndex + string.len(szSeparator)
       nSplitIndex = nSplitIndex + 1
    end
    return nSplitArray
end


LoadPoemfile()
LoadChaiziFile()

InitPoemPlusTable()
OutputFile3()
OutputFile4()
OutputFile5()

write2PoemFile()
OutputReverseTable()
OutputReverseTableCSV()
-- write2File()
