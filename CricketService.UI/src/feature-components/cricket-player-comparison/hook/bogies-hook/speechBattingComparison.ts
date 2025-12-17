interface PlayerStat {
  playerName: string;
  value: number | string;
}

export const battingComparisonSpeech = {
  "strike-rate-comparison-speech": (player1: PlayerStat, player2: PlayerStat): string => {
    const sr1 = Number(player1.value);
    const sr2 = Number(player2.value);
    
    if (isNaN(sr1) || isNaN(sr2)) {
      return `Strike rate comparison: ${player1.playerName}, ${player1.value}, ${player2.playerName}, ${player2.value}`;
    }

    const difference = Math.abs(sr1 - sr2);
    const percentageDiff = ((difference / Math.min(sr1, sr2)) * 100).toFixed(1);

    if (Math.abs(sr1 - sr2) < 1) {
      return `Both ${player1.playerName} and ${player2.playerName} have nearly identical strike rates at ${sr1.toFixed(2)} and ${sr2.toFixed(2)} respectively. This is a very close comparison!`;
    }

    if (sr1 > sr2) {
      return `${player1.playerName} has a superior strike rate of ${sr1.toFixed(2)}, which is ${difference.toFixed(2)} points higher than ${player2.playerName}'s ${sr2.toFixed(2)}. That's approximately ${percentageDiff} percent better!`;
    } else {
      return `${player2.playerName} dominates with a strike rate of ${sr2.toFixed(2)}, outpacing ${player1.playerName}'s ${sr1.toFixed(2)} by ${difference.toFixed(2)} points. That's about ${percentageDiff} percent superior!`;
    }
  },

  "average-comparison-speech": (player1: PlayerStat, player2: PlayerStat): string => {
    const avg1 = Number(player1.value);
    const avg2 = Number(player2.value);
    
    if (isNaN(avg1) || isNaN(avg2)) {
      return `Average comparison: ${player1.playerName}, ${player1.value}, ${player2.playerName}, ${player2.value}`;
    }

    const difference = Math.abs(avg1 - avg2);

    if (Math.abs(avg1 - avg2) < 1) {
      return `${player1.playerName} and ${player2.playerName} have remarkably similar averages at ${avg1.toFixed(2)} and ${avg2.toFixed(2)}. Both are consistent performers!`;
    }

    if (avg1 > avg2) {
      return `${player1.playerName} leads with an average of ${avg1.toFixed(2)}, which is ${difference.toFixed(2)} runs better than ${player2.playerName}'s ${avg2.toFixed(2)}. Excellent consistency from ${player1.playerName}!`;
    } else {
      return `${player2.playerName} excels with an average of ${avg2.toFixed(2)}, ${difference.toFixed(2)} runs ahead of ${player1.playerName}'s ${avg1.toFixed(2)}. A testament to ${player2.playerName}'s consistency!`;
    }
  },

  "runs-comparison-speech": (player1: PlayerStat, player2: PlayerStat): string => {
    const runs1 = Number(player1.value);
    const runs2 = Number(player2.value);
    
    if (isNaN(runs1) || isNaN(runs2)) {
      return `Runs comparison: ${player1.playerName}, ${player1.value} runs, ${player2.playerName}, ${player2.value} runs`;
    }

    const difference = Math.abs(runs1 - runs2);

    if (difference < 100) {
      return `${player1.playerName} has scored ${runs1} runs and ${player2.playerName} has ${runs2} runs. The difference is just ${difference} runs, making this a neck-and-neck race!`;
    }

    if (runs1 > runs2) {
      return `${player1.playerName} has amassed ${runs1} runs, leading ${player2.playerName} by ${difference} runs! ${player2.playerName} has scored ${runs2} runs in comparison.`;
    } else {
      return `${player2.playerName} is ahead with ${runs2} runs, a margin of ${difference} runs over ${player1.playerName}'s ${runs1} runs!`;
    }
  },

  "centuries-comparison-speech": (player1: PlayerStat, player2: PlayerStat): string => {
    const c1 = Number(player1.value);
    const c2 = Number(player2.value);
    
    if (isNaN(c1) || isNaN(c2)) {
      return `Centuries comparison: ${player1.playerName}, ${player1.value} hundreds, ${player2.playerName}, ${player2.value} hundreds`;
    }

    if (c1 === c2) {
      if (c1 === 0) {
        return `Neither ${player1.playerName} nor ${player2.playerName} have scored a hundred yet. Both are still hunting for that milestone!`;
      }
      return `Both ${player1.playerName} and ${player2.playerName} are tied with ${c1} hundred${c1 !== 1 ? 's' : ''} each. Equal brilliance!`;
    }

    if (c1 > c2) {
      if (c2 === 0) {
        return `${player1.playerName} has ${c1} hundred${c1 !== 1 ? 's' : ''} while ${player2.playerName} is yet to reach that milestone!`;
      }
      return `${player1.playerName} leads with ${c1} hundred${c1 !== 1 ? 's' : ''}, ${c1 - c2} more than ${player2.playerName}'s ${c2}!`;
    } else {
      if (c1 === 0) {
        return `${player2.playerName} has ${c2} hundred${c2 !== 1 ? 's' : ''} while ${player1.playerName} hasn't reached that mark yet!`;
      }
      return `${player2.playerName} dominates with ${c2} hundred${c2 !== 1 ? 's' : ''}, ${c2 - c1} ahead of ${player1.playerName}'s ${c1}!`;
    }
  },

  "matches-comparison-speech": (player1: PlayerStat, player2: PlayerStat): string => {
    const m1 = Number(player1.value);
    const m2 = Number(player2.value);
    
    if (isNaN(m1) || isNaN(m2)) {
      return `Matches comparison: ${player1.playerName}, ${player1.value} matches, ${player2.playerName}, ${player2.value} matches`;
    }

    const difference = Math.abs(m1 - m2);

    if (m1 === m2) {
      return `Both ${player1.playerName} and ${player2.playerName} have played exactly ${m1} matches. Same journey, different stories!`;
    }

    if (difference <= 5) {
      return `${player1.playerName} has played ${m1} matches while ${player2.playerName} has ${m2}. Almost equal experience on the field!`;
    }

    if (m1 > m2) {
      return `${player1.playerName} is the more experienced warrior with ${m1} matches under the belt, ${difference} more than ${player2.playerName}'s ${m2} matches!`;
    } else {
      return `${player2.playerName} brings more experience with ${m2} matches played, ${difference} ahead of ${player1.playerName}'s ${m1}!`;
    }
  },

  "fifties-comparison-speech": (player1: PlayerStat, player2: PlayerStat): string => {
    const f1 = Number(player1.value);
    const f2 = Number(player2.value);
    
    if (isNaN(f1) || isNaN(f2)) {
      return `Fifties comparison: ${player1.playerName}, ${player1.value} fifties, ${player2.playerName}, ${player2.value} fifties`;
    }

    if (f1 === f2) {
      if (f1 === 0) {
        return `Both ${player1.playerName} and ${player2.playerName} are yet to register a fifty. The wait continues!`;
      }
      return `${player1.playerName} and ${player2.playerName} are neck and neck with ${f1} half-centur${f1 !== 1 ? 'ies' : 'y'} each!`;
    }

    if (f1 > f2) {
      if (f2 === 0) {
        return `${player1.playerName} has ${f1} half-centur${f1 !== 1 ? 'ies' : 'y'} while ${player2.playerName} is still chasing that first fifty!`;
      }
      const diff = f1 - f2;
      return `${player1.playerName} shines with ${f1} half-centur${f1 !== 1 ? 'ies' : 'y'}, ${diff} more than ${player2.playerName}'s ${f2}. Consistent contributions from ${player1.playerName}!`;
    } else {
      if (f1 === 0) {
        return `${player2.playerName} has notched up ${f2} half-centur${f2 !== 1 ? 'ies' : 'y'} while ${player1.playerName} awaits that first milestone!`;
      }
      const diff = f2 - f1;
      return `${player2.playerName} takes the lead with ${f2} half-centur${f2 !== 1 ? 'ies' : 'y'}, ${diff} more than ${player1.playerName}'s ${f1}!`;
    }
  },

  "not-outs-comparison-speech": (player1: PlayerStat, player2: PlayerStat): string => {
    const no1 = Number(player1.value);
    const no2 = Number(player2.value);
    
    if (isNaN(no1) || isNaN(no2)) {
      return `Not outs comparison: ${player1.playerName}, ${player1.value}, ${player2.playerName}, ${player2.value}`;
    }

    if (no1 === no2) {
      if (no1 === 0) {
        return `Neither ${player1.playerName} nor ${player2.playerName} have remained not out. They're both playing aggressive cricket!`;
      }
      return `Both ${player1.playerName} and ${player2.playerName} have ${no1} not out${no1 !== 1 ? 's' : ''}. Equally reliable finishers!`;
    }

    if (no1 > no2) {
      const diff = no1 - no2;
      return `${player1.playerName} has stayed not out ${no1} times compared to ${player2.playerName}'s ${no2}. That's ${diff} more unbeaten innings! ${player1.playerName} knows how to see it through!`;
    } else {
      const diff = no2 - no1;
      return `${player2.playerName} has remained not out ${no2} times, ${diff} more than ${player1.playerName}'s ${no1}. A true finisher in ${player2.playerName}!`;
    }
  },

  "ducks-comparison-speech": (player1: PlayerStat, player2: PlayerStat): string => {
    const d1 = Number(player1.value);
    const d2 = Number(player2.value);
    
    if (isNaN(d1) || isNaN(d2)) {
      return `Ducks comparison: ${player1.playerName}, ${player1.value}, ${player2.playerName}, ${player2.value}`;
    }

    if (d1 === 0 && d2 === 0) {
      return `Excellent news! Neither ${player1.playerName} nor ${player2.playerName} have been dismissed for a duck. Both have avoided the dreaded zero!`;
    }

    if (d1 === d2) {
      return `Both ${player1.playerName} and ${player2.playerName} have ${d1} duck${d1 !== 1 ? 's' : ''}. Every batsman faces this challenge!`;
    }

    if (d1 < d2) {
      if (d1 === 0) {
        return `${player1.playerName} has avoided ducks completely while ${player2.playerName} has ${d2}. Clean slate for ${player1.playerName}!`;
      }
      const diff = d2 - d1;
      return `${player1.playerName} has ${d1} duck${d1 !== 1 ? 's' : ''}, ${diff} fewer than ${player2.playerName}'s ${d2}. Better start consistency from ${player1.playerName}!`;
    } else {
      if (d2 === 0) {
        return `${player2.playerName} has never been dismissed for a duck while ${player1.playerName} has ${d1}. Perfect record for ${player2.playerName}!`;
      }
      const diff = d1 - d2;
      return `${player2.playerName} has ${d2} duck${d2 !== 1 ? 's' : ''}, ${diff} fewer than ${player1.playerName}'s ${d1}. ${player2.playerName} shows better opening form!`;
    }
  },

  "fours-comparison-speech": (player1: PlayerStat, player2: PlayerStat): string => {
    const f1 = Number(player1.value);
    const f2 = Number(player2.value);
    
    if (isNaN(f1) || isNaN(f2)) {
      return `Fours comparison: ${player1.playerName}, ${player1.value} fours, ${player2.playerName}, ${player2.value} fours`;
    }

    const difference = Math.abs(f1 - f2);

    if (difference < 10) {
      return `${player1.playerName} has ${f1} boundaries while ${player2.playerName} has ${f2}. Almost identical boundary counts! Both love finding the gaps!`;
    }

    if (f1 > f2) {
      return `${player1.playerName} has smashed ${f1} boundaries, that's ${difference} more than ${player2.playerName}'s ${f2}! ${player1.playerName} is a master at piercing the field!`;
    } else {
      return `${player2.playerName} leads the boundary count with ${f2} fours, ${difference} ahead of ${player1.playerName}'s ${f1}! ${player2.playerName} knows how to find the fence!`;
    }
  },

  "sixes-comparison-speech": (player1: PlayerStat, player2: PlayerStat): string => {
    const s1 = Number(player1.value);
    const s2 = Number(player2.value);
    
    if (isNaN(s1) || isNaN(s2)) {
      return `Sixes comparison: ${player1.playerName}, ${player1.value} sixes, ${player2.playerName}, ${player2.value} sixes`;
    }

    if (s1 === 0 && s2 === 0) {
      return `Interesting! Neither ${player1.playerName} nor ${player2.playerName} have hit a six. They're playing it safe along the ground!`;
    }

    if (s1 === s2) {
      return `Both ${player1.playerName} and ${player2.playerName} have launched ${s1} maximum${s1 !== 1 ? 's' : ''}! Equal power hitters!`;
    }

    const difference = Math.abs(s1 - s2);

    if (s1 > s2) {
      if (s2 === 0) {
        return `${player1.playerName} has sent ${s1} balls into the crowd while ${player2.playerName} is yet to clear the boundary! ${player1.playerName} brings the power game!`;
      }
      return `${player1.playerName} has cleared the boundary ${s1} times, ${difference} more than ${player2.playerName}'s ${s2}! What a big hitter ${player1.playerName} is!`;
    } else {
      if (s1 === 0) {
        return `${player2.playerName} has launched ${s2} maximums while ${player1.playerName} hasn't cleared the ropes yet! ${player2.playerName} brings explosive power!`;
      }
      return `${player2.playerName} dominates the six-hitting with ${s2} maximums, ${difference} ahead of ${player1.playerName}'s ${s1}! ${player2.playerName} loves going big!`;
    }
  }
};
