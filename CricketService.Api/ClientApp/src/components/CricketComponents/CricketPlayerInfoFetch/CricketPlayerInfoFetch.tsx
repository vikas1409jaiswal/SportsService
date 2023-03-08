import { useState } from "react";
import { ProgressBar } from "../../common/ProgressBar";
import { usePlayerInfo } from "./usePlayerInfo";

export interface CricketPlayerInfoFetchProps {
    players: string[][];
    totalFetchPlayers: number,
    setTotalFetchPlayers: (count: number) => void;
}

export const CricketPlayerInfoFetch: React.FunctionComponent<
    CricketPlayerInfoFetchProps
> = ({ players, totalFetchPlayers, setTotalFetchPlayers }) => {
  
    const [isEnabledInput, toggleInput] = useState(true);
    const [isFetch, toggleFetch] = useState(false);

    const playersInfo = usePlayerInfo(players, isFetch);

    const fetchedLength = playersInfo.filter(x => x.fullName.length > 0).length;
    const fetchedPercent = fetchedLength * 100 / totalFetchPlayers;

    console.log(fetchedPercent)

    return <div>
        <input type='text' value={totalFetchPlayers} disabled={isEnabledInput} onChange={(e: any) => setTotalFetchPlayers(e.target.value)} />
        <button onClick={() => toggleInput(!isEnabledInput)}>+</button>
        <button onClick={() => toggleFetch(!isFetch)}>Fetch</button>
        <ProgressBar percent={fetchedPercent} />
        <h1>{fetchedLength} out of {totalFetchPlayers}</h1>
        <button onClick={() => downloadJsonData(playersInfo)} disabled={fetchedPercent !== 100}>Download</button>
    </div>;
    };

const downloadJsonData = (data: any) => {
    const filepath = `playersData.json`;

    const blob = new Blob([JSON.stringify(data, null, 2)], {
        type: "application/json",
    });

    const url = URL.createObjectURL(blob);

    const link = document.createElement("a");
    link.href = url;
    link.download = filepath;

    link.click();

    URL.revokeObjectURL(url);
};

