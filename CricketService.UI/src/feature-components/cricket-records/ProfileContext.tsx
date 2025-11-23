import { createContext, useContext } from "react";

export interface ProfileContextType {
  selectedProfile: string;
  setSelectedProfile: (profile: string) => void;
}

export const ProfileContext = createContext<ProfileContextType | undefined>(undefined);

export const useProfileContext = () => {
  const context = useContext(ProfileContext);
  if (!context) {
    throw new Error("useProfileContext must be used within a ProfileProvider");
  }
  return context;
};

export const ProfileProvider = ProfileContext.Provider;
