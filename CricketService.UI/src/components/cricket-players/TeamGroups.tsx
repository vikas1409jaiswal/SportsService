export type TeamsGroupProps = {
  // selectedTeamGroupsKey: string[];
  // setSelectedTeamGroupsKey: StringArrayModifiers;
};

export const TeamsGroup: React.FunctionComponent<TeamsGroupProps> = (props) => {
  // const handleSelected = (e: any) => {
  //   if (e.target.checked) {
  //     props.setSelectedTeamGroupsKey([
  //       ...props.selectedTeamGroupsKey,
  //       e.target.value,
  //     ]);
  //   } else {
  //     props.setSelectedTeamGroupsKey(
  //       props.selectedTeamGroupsKey.filter((f) => f !== e.target.value)
  //     );
  //   }
  // };

  return (
    <div className="teams-group-container">
      {/* {["Clubs", "Nations"].map((x) => (
        <div>
          {groups
            .filter((g) => g.includes(x))
            .map((group) => (
              <div>
                <input
                  type="checkbox"
                  value={group}
                  checked={props.selectedTeamGroupsKey.indexOf(group) !== -1}
                  onChange={handleSelected}
                />
                <span>{group}</span>
              </div>
            ))}
        </div>
      ))} */}
    </div>
  );
};
