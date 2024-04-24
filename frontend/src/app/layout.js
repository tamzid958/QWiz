import { Inter } from "next/font/google";
import "./globals.css";
import RootLayoutWrapper from "@/wrapper/client/RootLayoutWrapper";
import { getServerSession } from "next-auth";
import Layout from "@/components/Layout";
import "react-toastify/dist/ReactToastify.css";
import "@fontsource/roboto/300.css";
import "@fontsource/roboto/400.css";
import "@fontsource/roboto/500.css";
import "@fontsource/roboto/700.css";

const inter = Inter({ subsets: ["latin"] });

export const metadata = {
  title: "QWiz",
  description: "Question Bank Management",
};

export default async function RootLayout({ children }) {
  const session = await getServerSession();

  return (
    <html lang="en">
      <body className={inter.className}>
        <RootLayoutWrapper {...{ session }}>
          <Layout>{children}</Layout>
        </RootLayoutWrapper>
      </body>
    </html>
  );
}
