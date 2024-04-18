"use client";

import { getServerApi } from "@/utils/axios.settings";
import { SessionProvider } from "next-auth/react";
import { SWRConfig } from "swr";

const RootLayoutWrapper = ({ session: serverSession, children }) => {
  return (
    <SessionProvider session={serverSession}>
      <SWRConfig
        value={{
          provider: () => new Map(),
          fetcher: async ({ url, params }) =>
            await getServerApi({
              url,
              params,
            }).then((res) => res.data),
        }}
      >
        {children}
      </SWRConfig>
    </SessionProvider>
  );
};
export default RootLayoutWrapper;
