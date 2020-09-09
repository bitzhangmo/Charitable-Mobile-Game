-- poemfile = io.open("poem.txt", "rb")
-- chaiziFile = io.open("chaizi-jt.txt", "rb")

local poemTable = {}
local chaiziTable = {}
local reverseTable = {}

-- 加载poem.txt文件，并在拆字表中搜索对应字的拆字方法，并插入到诗词拆字表中
function loadPoemfile()
    for line in io.lines("poem.txt") do
        -- local poemLineTable = Split(line,"")
        -- print(#line)
        -- for i = 1,#line do

        -- end
        local i = 1
        while i < #line do
            local str = string.sub(line,i,i+1)  -- 中文长度为2
            i = i + 2
            table.insert(poemTable ,str)
        end
        -- print(line)
    end
    -- log_tree(poemTable)
end

function loadChaiziFile()
    for line in io.lines("chaizi-jt.txt") do    -- line 拆字表的每一行
        -- local subTable = string.split(line, ",")
        -- 生成结构：
        -- {
        --     七,      -- key_str
        --     一 乚,   
        --     丿 乚,
        -- }
        local subTable = Split(line, "	")      
        local key_str = subTable[1]
        local tempTable = {}
        for i = 2,#subTable do
            -- local wordTable = string.split(subTable[i], " ")
            local wordTable = Split(subTable[i], " ")
            for i,v in pairs(wordTable) do
                -- table.insert(reverseTable,v,key_str)
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
    -- log_tree(reverseTable)
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
                print("find"..word)
                local str = i..","
                for k,s in pairs(v) do
                    str = str..s[1]..s[2]..","      -- TODO：此处使用concat提升性能
                end
                str = str.."\n"
                print(str)
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

-- function string.split(str, sep)
--     local result = {}
--     if str == nil or sep == nil or type(str) ~= "string" or type(sep) ~= "string" then
--         return result
--     end
--     if #sep == 0 then
--         return result
--     end
--     local pattern = string.format("([^%s]*)", sep)
--     string.gsub(str, pattern, function(c) result[#result+1] = c end)
--     return result
-- end

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

OutputReverseTable()
loadPoemfile()
loadChaiziFile()
write2PoemFile()
OutputReverseTableCSV()
-- write2File()
